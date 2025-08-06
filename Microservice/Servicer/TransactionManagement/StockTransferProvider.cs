using Base.BaseService;
using Base.TransactionManagement;
using Context.WarehouseManagement;
using Core.BaseClass;
using Core.TransactionManagement;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Servicer.TransactionManagement
{
    public class StockTransferProvider : ICRUD_Service<StockTransfer, int>, IStockTransferProvider
    {
        private readonly DB_WarehouseManagement_Context _context;
        private readonly IConfiguration _configuration;
        private string _moduleDapper = "DB_Inventory_DAPPER";
        private const int TimeoutInSeconds = 240;

        public StockTransferProvider(DB_WarehouseManagement_Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResultService<IEnumerable<StockTransfer>>> GetAll()
        {
            var response = new ResultService<IEnumerable<StockTransfer>>();
            string connectionStrig = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionStrig))
                {
                    await sqlConnection.OpenAsync();
                    var result = await sqlConnection.QueryAsync<StockTransfer>("StockTransfer_GetAll",
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: TimeoutInSeconds);

                    response.Code = "0";
                    response.Message = "Success";
                    response.Data = result;
                    return response;

                }
            }
            catch (SqlException ex)
            {
                response.Code = "1";
                response.Message = $"{ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "999";
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<StockTransfer>> Get(int id)
        {
            var response = new ResultService<StockTransfer>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@ID", id);
                    var result = await sqlConnection.QuerySingleOrDefaultAsync<StockTransfer>("StockTransfer_GetByID",
                        param,
                        commandType: CommandType.StoredProcedure,
                           commandTimeout: TimeoutInSeconds);
                    if (result == null)
                    {
                        return new ResultService<StockTransfer>()
                        {
                            Code = "-1",
                            Message = "Entity not found",
                            Data = null
                        };
                    }
                    response.Code = "0";// Success
                    response.Message = "Success";
                    response.Data = result;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.Code = "1";
                response.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Code = "2";
                response.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "999";
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<StockTransfer>> Create(StockTransfer entity)
        {
            var response = new ResultService<StockTransfer>();
            if (entity == null)
            {
                return new ResultService<StockTransfer>()
                {
                    Code = "-1",
                    Message = "Entity is null"
                };
            }

            using (var connection = _context.Database.BeginTransaction())
            {
                try
                {
                    entity.RowPointer = Guid.Empty;
                    entity.ID = 0;
                    await _context.StockTransfers.AddAsync(entity);
                    await _context.SaveChangesAsync();
                    await connection.CommitAsync();

                    response.Code = "0";
                    response.Message = "Create new instance successfully";
                    response.Data = entity;
                    return response;

                }
                catch (SqlException ex)
                {
                    response.Code = "2";
                    response.Message = $"Something error or conflict happend : {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (DbUpdateException ex)
                {
                    response.Code = "3";
                    response.Message = $"Concurrency error conflict happend : {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (OperationCanceledException ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "5";
                    response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (Exception ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "6";
                    response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                    return response;
                }
            }
        }

        public async Task<ResultService<StockTransfer>> CreateByDapper(StockTransfer entity)
        {
            var response = new ResultService<StockTransfer>();
            if (entity == null)
            {
                return new ResultService<StockTransfer>()
                {
                    Code = "1",
                    Message = "Entity is not valid"
                };
            }
            try
            {
                string Message = string.Empty;
                entity.TransferCode = !entity.TransferCode.Contains("ST") ? string.Empty : entity.TransferCode;
                List<StockTransfer> lst = new List<StockTransfer>();
                lst.Add(entity);
                DataTable dtHeader = General.ConvertToDataTable(lst);
                string conn = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@createdBy", entity.CreatedBy);

                    param.Add("@udtt_Header", dtHeader.AsTableValuedParameter("UDTT_StockTransferHeader"));
                    param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<StockTransfer>("StockTransfer_Create",
                            param,
                            commandType: CommandType.StoredProcedure,
                                commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        response.Code = "0";
                        response.Message = "Save Successfully";
                    }
                    else
                    {
                        response.Code = "0";
                        response.Message = resultMessage;
                    }

                    return response;
                }
            }
            catch (SqlException sqlex)
            {

                response.Code = "2";
                response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {

                response.Code = "3";
                response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (DbUpdateException ex)
            {

                response.Code = "4";
                response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (OperationCanceledException ex)
            {

                response.Code = "5";
                response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {

                response.Code = "6";
                response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<StockTransfer_Param>> CreateHeaderAndDetail(StockTransfer_Param entity)
        {
            var response = new ResultService<StockTransfer_Param>();
            if (entity == null)
            {
                return new ResultService<StockTransfer_Param>()
                {
                    Code = "1",
                    Message = "Entity is not valid"
                };
            }
            try
            {
                string Message = string.Empty;
                string conn = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("CreateBy", entity.CreatedBy);
                    param.Add("@udtt_Header", General.ConvertToDataTable(entity.StockTransfers).AsTableValuedParameter("UDTT_StockTransferHeader"));
                    param.Add("@udtt_Detail", General.ConvertToDataTable(entity.StockTransferDetails).AsTableValuedParameter("UDTT_StockTransferDetail"));
                    param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<StockTransfer>("StockTransfer_Create",
                            param,
                            commandType: CommandType.StoredProcedure,
                                commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        response.Code = "0";
                        response.Message = "Save Successfully";
                    }
                    else
                    {
                        response.Code = "0";
                        response.Message = resultMessage;
                    }

                    return response;
                }
            }
            catch (SqlException sqlex)
            {

                response.Code = "2";
                response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {

                response.Code = "3";
                response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (DbUpdateException ex)
            {

                response.Code = "4";
                response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (OperationCanceledException ex)
            {

                response.Code = "5";
                response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {

                response.Code = "6";
                response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<string>> Delete(int id)
        {
            ResultService<string> resultService = new ResultService<string>();
            try
            {
                var entity = await Get(id);
                if (entity.Code == "-1")
                {
                    return new ResultService<string>()
                    {
                        Code = entity.Code,
                        Data = null,
                        Message = "Entity not found"
                    };
                }

                _context.StockTransfers.Remove(entity.Data);
                await _context.SaveChangesAsync();
                resultService.Code = "0";
                resultService.Message = "Entity deleted successfully";
                return resultService;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResultService<string>()
                {
                    Code = "2",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };

            }
            catch (DbUpdateException ex)
            {
                return new ResultService<string>()
                {
                    Code = "3",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (OperationCanceledException ex)
            {
                return new ResultService<string>()
                {
                    Code = "4",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (SqlException ex)
            {
                return new ResultService<string>()
                {
                    Code = "5",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResultService<string>()
                {
                    Code = "6",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
        }

        public async Task<ResultService<string>> DeleteByDapper(string stCode)
        {
            ResultService<string> resultService = new ResultService<string>();
            try
            {
                string Message = string.Empty;
                string conn = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@TransferCode", stCode);
                    param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<StockTransfer>("StockTransfer_Delete",
                            param,
                            commandType: CommandType.StoredProcedure,
                                commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("Ok"))
                    {
                        resultService.Code = "0";
                        resultService.Message = "Delete Successfully";
                    }
                    else
                    {
                        resultService.Code = "0";
                        resultService.Message = resultMessage;
                    }

                    return resultService;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResultService<string>()
                {
                    Code = "2",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };

            }
            catch (DbUpdateException ex)
            {
                return new ResultService<string>()
                {
                    Code = "3",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (OperationCanceledException ex)
            {
                return new ResultService<string>()
                {
                    Code = "4",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (SqlException ex)
            {
                return new ResultService<string>()
                {
                    Code = "5",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResultService<string>()
                {
                    Code = "6",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
        }

        public async Task<ResultService<string>> DeleteDetail(List<StockTransferDetail> entity)
        {
            ResultService<string> resultService = new ResultService<string>();
            try
            {
                string Message = string.Empty;
                string conn = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@udtt_Detail", General.ConvertToDataTable(entity).AsTableValuedParameter("UDTT_StockTransferDetail"));
                    param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<StockTransfer>("StockTransfer_Delete_Multi",
                            param,
                            commandType: CommandType.StoredProcedure,
                                commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        resultService.Code = "0";
                        resultService.Message = "Delete Successfully";
                    }
                    else
                    {
                        resultService.Code = "-999";
                        resultService.Message = "Failed";
                    }

                    return resultService;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResultService<string>()
                {
                    Code = "2",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (DbUpdateException ex)
            {
                return new ResultService<string>()
                {
                    Code = "3",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (OperationCanceledException ex)
            {
                return new ResultService<string>()
                {
                    Code = "4",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (SqlException ex)
            {
                return new ResultService<string>()
                {
                    Code = "5",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResultService<string>()
                {
                    Code = "6",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
        }

        public async Task<ResultService<StockTransfer>> Update(StockTransfer entity)
        {
            var response = new ResultService<StockTransfer>();
            var getID = await Get(entity.ID);
            if (getID.Code == "-1")
            {
                return new ResultService<StockTransfer>()
                {
                    Code = getID.Code,
                    Message = "Entity not found"
                };
            }
            var newEntity = await _context.StockTransfers.FindAsync(getID.Data.RowPointer);
            if (newEntity != null)
            {
                return new ResultService<StockTransfer>()
                {
                    Code = "-1",
                    Message = "Entity not found"
                };
            }
            using (var connection = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    newEntity.TransferCode = entity.TransferCode;
                    newEntity.FromWarehouseCode = entity.FromWarehouseCode;
                    newEntity.ToWarehouseCode = entity.ToWarehouseCode;
                    newEntity.TransactionTypeCode = entity.TransactionTypeCode;
                    newEntity.Notes = entity.Notes;
                    newEntity.UpdatedDate = DateTime.UtcNow;
                    newEntity.UpdatedBy = entity.UpdatedBy;

                    await _context.SaveChangesAsync();
                    await connection.CommitAsync();
                    response.Code = "0";
                    response.Data = newEntity;
                    response.Message = "Update entity Successfully";
                    return response;
                }
                catch (SqlException sqlex)
                {
                    await connection.RollbackAsync();
                    response.Code = "2";
                    response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                    return response;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "3";
                    response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (DbUpdateException ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "4";
                    response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (OperationCanceledException ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "5";
                    response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (Exception ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "6";
                    response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                    return response;
                }

            }

        }

        public async Task<ResultService<string>> Delete_HeaderAndDetail(int stID)
        {
            ResultService<string> resultService = new ResultService<string>();
            try
            {
                string Message = string.Empty;
                string conn = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("stID", stID);
                    param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<StockTransfer>("StockTransfer_Delete_HeaderAndDetail",
                            param,
                            commandType: CommandType.StoredProcedure,
                                commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        resultService.Code = "0";
                        resultService.Message = "Delete Successfully";
                    }
                    else
                    {
                        resultService.Code = "-999";
                        resultService.Message = "Failed";
                    }

                    return resultService;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new ResultService<string>()
                {
                    Code = "2",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };

            }
            catch (DbUpdateException ex)
            {
                return new ResultService<string>()
                {
                    Code = "3",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (OperationCanceledException ex)
            {
                return new ResultService<string>()
                {
                    Code = "4",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (SqlException ex)
            {
                return new ResultService<string>()
                {
                    Code = "5",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResultService<string>()
                {
                    Code = "6",
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
        }

        public Task<ResultService<string>> Save(StockTransfer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultService<IEnumerable<StockTransferDetail>>> GetDetailByStockTransferID(string stCode)
        {
            var response = new ResultService<IEnumerable<StockTransferDetail>>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@TFCode", stCode);
                    var result = await sqlConnection.QueryAsync<StockTransferDetail>("StockTransferDetail_GetByStockTransferID",
                        param,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: TimeoutInSeconds);
                    response.Code = "0";
                    response.Message = "Success";
                    response.Data = result;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.Code = "1";
                response.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Code = "2";
                response.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "999";
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<StockTransfer_Param>> Save(StockTransfer_Param entity)
        {
            var response = new ResultService<StockTransfer_Param>();
            if (entity == null)
            {
                return new ResultService<StockTransfer_Param>()
                {
                    Code = "1",
                    Message = "Entity is not valid"
                };
            }
            try
            {
                string Message = string.Empty;

                string conn = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@CreatedBy", entity.CreatedBy);
                    param.Add("@udtt_Header", General.ConvertToDataTable(entity.StockTransfers).AsTableValuedParameter("UDTT_StockTransferHeader"));
                    param.Add("@udtt_Detail", General.ConvertToDataTable(entity.StockTransferDetails).AsTableValuedParameter("UDTT_StockTransferDetail"));
                    param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<GoodsReceiptNote>("StockTransfer_Create",
                       param,
                       commandType: CommandType.StoredProcedure,
                          commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        response.Code = "0"; // Success
                        response.Message = "Save Successfully";
                    }
                    else
                    {
                        response.Code = "-999"; // Success
                        response.Message = "Failed";
                    }

                    return response;

                }
            }
            catch (SqlException sqlex)
            {

                response.Code = "2";
                response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {

                response.Code = "3";
                response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (DbUpdateException ex)
            {

                response.Code = "4";
                response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (OperationCanceledException ex)
            {

                response.Code = "5";
                response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {

                response.Code = "6";
                response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }
    }
}
