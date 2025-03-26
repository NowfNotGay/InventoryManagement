using Base.BaseService;
using Base.WarehouseManagement;
using Context.WarehouseManagement;
using Core.BaseClass;
using Core.WarehouseManagement;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Servicer.WarehouseManagement
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

            }
            catch (Exception)
            {

            }
        }

        public Task<ResultService<StockTransfer_Param>> CreateHeaderAndDetail(StockTransfer_Param entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultService<string>> Delete(int id)
        {
            ResultService<string> resultService = new ResultService<string>();
            try
            {
                var entity = await this.Get(id);
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

        public Task<ResultService<string>> DeleteByDapper(string stCode)
        {
            throw new NotImplementedException();
        }

        public Task<ResultService<string>> DeleteDetail(List<StockTransferDetail> entity)
        {
            throw new NotImplementedException();
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
                    newEntity.FromWarehouseID = entity.FromWarehouseID;
                    newEntity.ToWarehouseID = entity.ToWarehouseID;
                    newEntity.TransactionTypeID = entity.TransactionTypeID;
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

        public Task<ResultService<string>> Delete_HeaderAndDetail(int stID)
        {
            throw new NotImplementedException();
        }
    }
}
