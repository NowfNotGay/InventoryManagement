﻿using Base.BaseService;
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
using System.Text.RegularExpressions;
using static Dapper.SqlMapper;

namespace Servicer.WarehouseManagement
{
    public class GoodsReceiptNoteProvider : ICRUD_Service<GoodsReceiptNote, int>, IGoodsReceiptNoteProvider
    {
        private readonly DB_WarehouseManagement_Context _Context;
        private readonly IConfiguration _configuration;
        private string _moduleDapper = "DB_Inventory_DAPPER";
        private const int TimeoutInSeconds = 240;

        public GoodsReceiptNoteProvider(DB_WarehouseManagement_Context context, IConfiguration configuration)
        {
            _Context = context;
            _configuration = configuration;

        }

        #region GoodsReceiptNote
        public async Task<ResultService<IEnumerable<GoodsReceiptNote>>> GetAll()
        {
            var response = new ResultService<IEnumerable<GoodsReceiptNote>>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var result = await sqlConnection.QueryAsync<GoodsReceiptNote>("GoodsReceiptNote_GetAll",
                        commandType: CommandType.StoredProcedure,
                           commandTimeout: TimeoutInSeconds);

                    response.Code = ResponseCode.Success.ToString();// Success
                    response.Message = "Success";
                    response.Data = result;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString();
                response.Message = $"SQL Exception: {sqlEx.GetType()} - {sqlEx.Message}";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Code = ResponseCode.InvalidInput.ToString();
                response.Message = $"Configuration error: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.UnhandledError.ToString();
                response.Message = $"Unexpected error: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }
        public async Task<ResultService<GoodsReceiptNote>> Get(int id)
        {
            var response = new ResultService<GoodsReceiptNote>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@ID", id);
                    var result = await sqlConnection.QuerySingleOrDefaultAsync<GoodsReceiptNote>("GoodsReceiptNote_GetByID",
                        param,
                        commandType: CommandType.StoredProcedure,
                           commandTimeout: TimeoutInSeconds);
                    if (result == null)
                    {
                        return new ResultService<GoodsReceiptNote>()
                        {
                            Code = ResponseCode.NotFound.ToString(),
                            Message = "Entity not found",
                            Data = null
                        };
                    }
                    response.Code = ResponseCode.Success.ToString();// Success
                    response.Message = "Success";
                    response.Data = result;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString();
                response.Message = $"SQL Exception: {sqlEx.GetType()} - {sqlEx.Message}";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Code = ResponseCode.InvalidInput.ToString();
                response.Message = $"Configuration error: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.UnhandledError.ToString();
                response.Message = $"Unexpected error: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }
        public async Task<ResultService<GoodsReceiptNote>> Create(GoodsReceiptNote entity)
        {
            var response = new ResultService<GoodsReceiptNote>();
            if (entity == null)
            {
                return new ResultService<GoodsReceiptNote>()
                {
                    Code = ResponseCode.InvalidInput.ToString()
                };
                throw new Exception("Entity is not valid");
            }

            using (var connection = _Context.Database.BeginTransaction())
            {
                try
                {
                    entity.RowPointer = Guid.Empty;
                    entity.ID = 0;
                    await _Context.GoodsReceiptNotes.AddAsync(entity);
                    await _Context.SaveChangesAsync();
                    await connection.CommitAsync();

                    response.Code = ResponseCode.Success.ToString();// Success
                    response.Message = "Create new Instance Successfully";
                    response.Data = entity;
                    return response;
                }
                catch (SqlException sqlex)
                {
                    await connection.RollbackAsync();
                    //lỗi xảy ra khi có sự xung đột giữa các bản ghi trong cơ sở dữ liệu khi cố gắng cập nhật.
                    response.Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString();
                    response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                    return response;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await connection.RollbackAsync();
                    //lỗi xảy ra khi có sự xung đột giữa các bản ghi trong cơ sở dữ liệu khi cố gắng cập nhật.
                    response.Code = "3";
                    response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (DbUpdateException ex)
                {
                    await connection.RollbackAsync();
                    //lỗi xảy ra khi không thể cập nhật cơ sở dữ liệu, có thể do các vấn đề về dữ liệu hoặc các ràng buộc.
                    response.Code = "4";
                    response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (OperationCanceledException ex)
                {
                    await connection.RollbackAsync();
                    //Lỗi xuất hiện do timeout hoặc yêu cầu dừng quá trình.
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
        public async Task<ResultService<GoodsReceiptNote>> Update(GoodsReceiptNote entity)
        {
            var response = new ResultService<GoodsReceiptNote>();

            var getEntityID = await this.Get(entity.ID);
            if (getEntityID.Code == "-1")
            {
                return new ResultService<GoodsReceiptNote>()
                {
                    Code = getEntityID.Code,
                    Message = "Entity not found"
                };

            }
            var newEntity = await _Context.GoodsReceiptNotes.FindAsync(getEntityID.Data.RowPointer);
            if (newEntity == null)
            {
                return new ResultService<GoodsReceiptNote>()
                {
                    Code = "-1",
                    Message = "Entity not found"
                };

            }
            using (var connection = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    newEntity.WarehouseID = entity.WarehouseID;
                    newEntity.UpdatedDate = DateTime.UtcNow;
                    newEntity.UpdatedBy = entity.UpdatedBy;
                    newEntity.SupplierID = entity.SupplierID;
                    newEntity.TransactionTypeID = entity.TransactionTypeID;
                    newEntity.ReceiptDate = entity.ReceiptDate;
                    newEntity.Notes = entity.Notes;

                    await _Context.SaveChangesAsync();
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

                _Context.GoodsReceiptNotes.Remove(entity.Data);
                await _Context.SaveChangesAsync();
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
        public async Task<ResultService<GoodsReceiptNote>> CreateByDapper(GoodsReceiptNote entity)
        {
            var response = new ResultService<GoodsReceiptNote>();

            if (entity == null)
            {
                response.Code = ResponseCode.InvalidInput.ToString();
                response.Message = "Entity is not valid";
                return response;
            }

            try
            {
                string message = string.Empty;
                entity.GRNCode = !entity.GRNCode.Contains("GRN") ? string.Empty : entity.GRNCode;

                List<GoodsReceiptNote> lst = new List<GoodsReceiptNote> { entity };
                DataTable dtHeader = General.ConvertToDataTable(lst);

                string conn = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();

                    var param = new DynamicParameters();
                    param.Add("@CreatedBy", entity.CreatedBy);
                    param.Add("@udtt_Header", dtHeader.AsTableValuedParameter("UDTT_GoodsReceiptNoteHeader"));
                    param.Add("@Message", message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                    await connection.QueryAsync<GoodsReceiptNote>(
                        "GoodsReceiptNote_Create",
                        param,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: TimeoutInSeconds
                    );

                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK", StringComparison.OrdinalIgnoreCase))
                    {
                        response.Code = ResponseCode.Success.ToString();
                        response.Message = "Save Successfully";
                    }
                    else
                    {
                        response.Code = ResponseCode.NotFound.ToString(); // hoặc FailWhileExecutingStoredProcedure
                        response.Message = $"Save Failed: {resultMessage}";
                    }

                    return response;
                }
            }
            catch (SqlException sqlex)
            {
                response.Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString();
                response.Message = $"Something went wrong with the database: {sqlex.GetType()} - {sqlex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.UnhandledError.ToString();
                response.Message = $"Unexpected error: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }
        public async Task<ResultService<string>> DeleteByDapper(string grnCode)
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
                    param.Add("@GRNCode", grnCode);
                    param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<GoodsReceiptNote>("GoodsReceiptNote_Delete",
                       param,
                       commandType: CommandType.StoredProcedure,
                          commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        resultService.Code = ResponseCode.Success.ToString(); // Success
                        resultService.Message = "Deleted Successfully";
                    }
                    else
                    {
                        resultService.Code = ResponseCode.NotFound.ToString();
                        resultService.Message = "Not Found";
                    }

                    return resultService;
                }
            }
            catch (SqlException ex)
            {
                return new ResultService<string>()
                {
                    Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString(),
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResultService<string>()
                {
                    Code = ResponseCode.UnhandledError.ToString(),
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
        }
        #endregion

        #region GoodsReceiptNoteLine
        public async Task<ResultService<IEnumerable<GoodsReceiptNoteLine>>> GetLineByRefCode(string GRNcode)
        {
            var response = new ResultService<IEnumerable<GoodsReceiptNoteLine>>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var parameter = new DynamicParameters();
                    parameter.Add("@GRNCode", GRNcode);
                    var result = await sqlConnection.QueryAsync<GoodsReceiptNoteLine>("GoodsReceiptNoteDetail_GetByGRNCode",
                        parameter,
                        commandType: CommandType.StoredProcedure,
                           commandTimeout: TimeoutInSeconds);

                    response.Code = ResponseCode.Success.ToString();// Success
                    response.Message = "Success";
                    response.Data = result;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString();
                response.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Code = ResponseCode.FailToConnectDB.ToString();
                response.Message = $"An error occurred while trying to connect to your database Server \n {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString();
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }
        public async Task<ResultService<string>> DeleteLine(List<GoodsReceiptNoteLine> entity)
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
                    param.Add("@udtt_Detail", General.ConvertToDataTable(entity).AsTableValuedParameter("UDTT_GoodsReceiptNoteDetail"));
                    param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<GoodsReceiptNote>("GoodsReceiptNoteDetail_Delete_Multi",
                       param,
                       commandType: CommandType.StoredProcedure,
                          commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        resultService.Code = ResponseCode.Success.ToString(); // Success
                        resultService.Message = "Deleted Successfully";
                    }
                    else
                    {
                        resultService.Code = ResponseCode.UnhandledError.ToString();
                        resultService.Message = "Failed";
                    }

                    return resultService;
                }
            }
            catch (SqlException ex)
            {
                return new ResultService<string>()
                {
                    Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString(),
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResultService<string>()
                {
                    Code = ResponseCode.UnhandledError.ToString(),
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
        }
        #endregion

        #region GoodsReceiptNoteHeaderAndDetail

        public async Task<ResultService<string>> Delete_HeaderAndDetail(int grnID)
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
                    param.Add("@grnID", grnID);
                    param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<GoodsReceiptNote>("GoodsReceiptNote_Delete_HeaderAndDetail",
                       param,
                       commandType: CommandType.StoredProcedure,
                          commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        resultService.Code = ResponseCode.Success.ToString(); // Success
                        resultService.Message = "Deleted Successfully";
                    }
                    else
                    {
                        resultService.Code = ResponseCode.UnhandledError.ToString();
                        resultService.Message = "Failed";
                    }

                    return resultService;
                }
            }
            catch (SqlException ex)
            {
                return new ResultService<string>()
                {
                    Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString(),
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResultService<string>()
                {
                    Code = ResponseCode.UnhandledError.ToString(),
                    Data = null,
                    Message = $"{ex.GetType()}, {ex.Message}"
                };
            }
        }

        public async Task<ResultService<GoodsReceiptNote_Param>> Save(GoodsReceiptNote_Param entity)
        {
            var response = new ResultService<GoodsReceiptNote_Param>();
            if (entity == null)
            {
                return new ResultService<GoodsReceiptNote_Param>()
                {
                    Code = ResponseCode.InvalidInput.ToString(),
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
                    param.Add("@udtt_Header", General.ConvertToDataTable(entity.GRNs).AsTableValuedParameter("UDTT_GoodsReceiptNoteHeader"));
                    param.Add("@udtt_Detail", General.ConvertToDataTable(entity.GRNLines).AsTableValuedParameter("UDTT_GoodsReceiptNoteDetail"));
                    param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    await connection.QueryAsync<GoodsReceiptNote>("GoodsReceiptNote_Create",
                       param,
                       commandType: CommandType.StoredProcedure,
                          commandTimeout: TimeoutInSeconds);
                    var resultMessage = param.Get<string>("@Message");

                    if (resultMessage.Contains("OK"))
                    {
                        response.Code = ResponseCode.Success.ToString(); // Success
                        response.Message = "Save Successfully";
                    }
                    else
                    {
                        response.Code = ResponseCode.UnhandledError.ToString(); // Success
                        response.Message = "Failed";
                    }

                    return response;

                }
            }
            catch (SqlException sqlex)
            {

                response.Code = ResponseCode.FailWhileExecutingStoredProcedure.ToString();
                response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                return response;
            }
            catch (Exception ex)
            {

                response.Code = ResponseCode.UnhandledError.ToString();
                response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }
        #endregion

    }
}
