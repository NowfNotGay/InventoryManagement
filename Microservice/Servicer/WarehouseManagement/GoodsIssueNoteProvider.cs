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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Servicer.WarehouseManagement;
public class GoodsIssueNoteProvider : ICRUD_Service<GoodsIssueNote, int>, IGoodsIssueNoteProvider
{
    private readonly DB_WarehouseManagement_Context _dB;
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString;
    private const int TimeoutInSeconds = 240;

    public GoodsIssueNoteProvider(DB_WarehouseManagement_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER")!);

    }

    public async Task<ResultService<GoodsIssueNote>> Create(GoodsIssueNote entity) //done
    {
        var response = new ResultService<GoodsIssueNote>();
        if (entity == null)
        {
            return new ResultService<GoodsIssueNote>()
            {
                Code = "1",
                Message = "Entity is not valid"
            };
        }

        using (var connection = _dB.Database.BeginTransaction())
        {
            try
            {
                entity.RowPointer = Guid.Empty;
                entity.ID = 0;
                await _dB.GoodsIssueNotes.AddAsync(entity);
                await _dB.SaveChangesAsync();
                await connection.CommitAsync();

                response.Code = "0";// Success
                response.Message = "Create new Instance Successfully";
                response.Data = entity;
                return response;
            }
            catch (SqlException sqlex)
            {
                await connection.RollbackAsync();
                //lỗi xảy ra khi có sự xung đột giữa các bản ghi trong cơ sở dữ liệu khi cố gắng cập nhật.
                response.Code = "2";
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

    public async Task<ResultService<GoodsIssueNote>> Update(GoodsIssueNote entity) //done
    {
        var response = new ResultService<GoodsIssueNote>();

        var getEntityID = await this.Get(entity.ID);
        if (getEntityID.Code == "-1")
        {
            return new ResultService<GoodsIssueNote>()
            {
                Code = getEntityID.Code,
                Message = "Entity not found"
            };

        }
        var newEntity = await _dB.GoodsIssueNotes.FindAsync(getEntityID.Data.RowPointer);
        if (newEntity == null)
        {
            return new ResultService<GoodsIssueNote>()
            {
                Code = "-1",
                Message = "Entity not found"
            };

        }
        using (var connection = await _dB.Database.BeginTransactionAsync())
        {
            try
            {
                newEntity.WarehouseID = entity.WarehouseID;
                newEntity.UpdatedDate = DateTime.UtcNow;
                newEntity.UpdatedBy = entity.UpdatedBy;
                newEntity.CustomerID = entity.CustomerID;
                newEntity.TransactionTypeID = entity.TransactionTypeID;
                newEntity.IssueDate = entity.IssueDate;
                newEntity.Notes = entity.Notes;

                await _dB.SaveChangesAsync();
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

    public async Task<ResultService<string>> Delete(int id) //done
    {
        ResultService<string> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await Get(id);
                if (!obj.Code.Equals("0"))
                {
                    result.Message = obj.Message;
                    result.Code = obj.Code;
                    result.Data = "false";
                    return result;

                }
                _dB.GoodsIssueNotes.Remove(obj.Data);
                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to delete data";
                    result.Code = "-1";
                    result.Data = "false";
                    return result;
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = "true";
                return result;
            }
            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();
                result.Code = "1";
                result.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return result;
            }
            catch (ArgumentException ex)
            {
                await transaction.RollbackAsync();
                result.Code = "2";
                result.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }

        }
    }

    public async Task<ResultService<GoodsIssueNote>> Get(int id) //done
    {
        var response = new ResultService<GoodsIssueNote>();
   
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(_dapperConnectionString))
            {
                await sqlConnection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@ID", id);
                var result = await sqlConnection.QuerySingleOrDefaultAsync<GoodsIssueNote>("GoodsIssueNote_GetByID",
                    param,
                    commandType: CommandType.StoredProcedure,
                       commandTimeout: TimeoutInSeconds);
                if (result == null)
                {
                    return new ResultService<GoodsIssueNote>()
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

    public async Task<ResultService<IEnumerable<GoodsIssueNote>>> GetAll() //done
    {
        var response = new ResultService<IEnumerable<GoodsIssueNote>>();
 
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(_dapperConnectionString))
            {
                await sqlConnection.OpenAsync();
                var result = await sqlConnection.QueryAsync<GoodsIssueNote>("GoodsIssueNote_GetAll",
                    commandType: CommandType.StoredProcedure,
                       commandTimeout: TimeoutInSeconds);

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

    //public async Task<ResultService<GoodsIssueNote>> SaveByDapper(GoodsIssueNote entity)
    //{
    //    var response = new ResultService<GoodsIssueNote>();
    //    if (entity == null)
    //    {
    //        return new ResultService<GoodsIssueNote>()
    //        {
    //            Code = "1",
    //            Message = "Entity is not valid"
    //        };
    //    }
    //    try
    //    {
    //        string Message = string.Empty;
    //        entity.GINCode = !entity.GINCode.Contains("GIN") ? string.Empty : entity.GINCode;
    //        List<GoodsIssueNote> lst = new List<GoodsIssueNote>();
    //        lst.Add(entity);
    //        DataTable dtHeader = General.ConvertToDataTable(lst);

    //        using (var connection = new SqlConnection(_dapperConnectionString))
    //        {

    //            await connection.OpenAsync();
    //            var param = new DynamicParameters();
    //            param.Add("@CreatedBy", entity.CreatedBy);
    //            param.Add("@udtt_Header", dtHeader.AsTableValuedParameter("UDTT_GoodsIssueNoteHeader"));
    //            param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
    //            await connection.QueryAsync<GoodsIssueNote>("GoodsIssueNote_Save",
    //               param,
    //               commandType: CommandType.StoredProcedure,
    //                  commandTimeout: TimeoutInSeconds);
    //            var resultMessage = param.Get<string>("@Message");

    //            if (resultMessage.Contains("OK"))
    //            {
    //                response.Code = "0"; // Success
    //                response.Message = "Save Successfully";
    //            }
    //            else
    //            {
    //                response.Code = "-999"; // Success
    //                response.Message = "Failed";
    //            }

    //            return response;

    //        }
    //    }
    //    catch (SqlException sqlex)
    //    {

    //        response.Code = "2";
    //        response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
    //        return response;
    //    }
    //    catch (DbUpdateConcurrencyException ex)
    //    {

    //        response.Code = "3";
    //        response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
    //        return response;
    //    }
    //    catch (DbUpdateException ex)
    //    {

    //        response.Code = "4";
    //        response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
    //        return response;
    //    }
    //    catch (OperationCanceledException ex)
    //    {

    //        response.Code = "5";
    //        response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
    //        return response;
    //    }
    //    catch (Exception ex)
    //    {

    //        response.Code = "6";
    //        response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
    //        return response;
    //    }
    //}
    public async Task<ResultService<GoodsIssueNote>> SaveByDapper(GoodsIssueNote entity)
    {
        var response = new ResultService<GoodsIssueNote>();
        if (entity == null)
        {
            return new ResultService<GoodsIssueNote> { Code = "1", Message = "Entity is not valid" };
        }

        try
        {
            if (string.IsNullOrEmpty(entity.GINCode) || !entity.GINCode.Contains("GIN"))
            {
                entity.GINCode = string.Empty; // Comment: GINCode phải chứa "GIN" theo nghiệp vụ
            }

            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_Header", General.ConvertToDataTable(new List<GoodsIssueNote> { entity }).AsTableValuedParameter("UDTT_GoodsIssueNoteHeader"));
                param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                var savedEntity = await connection.QuerySingleOrDefaultAsync<GoodsIssueNote>("GoodsIssueNote_Save",
                    param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage?.Contains("OK", StringComparison.OrdinalIgnoreCase) == true)
                {
                    response.Code = "0";
                    response.Message = "Save Successfully";
                }
                else
                {
                    response.Code = "-999";
                    response.Message = "Failed: " + resultMessage;
                }
                return response;
            }
        }
        catch (SqlException sqlex)
        {
            response.Code = "2";
            response.Message = $"Database error: {sqlex.Message}";
            return response;
        }
        catch (Exception ex)
        {
            response.Code = "6";
            response.Message = $"Unexpected error: {ex.Message}";
            return response;
        }
    }
    public async Task<ResultService<string>> DeleteByDapper(string ginCode) //done
    {
        ResultService<string> resultService = new();
        try
        {
            string Message = string.Empty;

            using (var connection = new SqlConnection(_dapperConnectionString))
            {

                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@GINCode", ginCode);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<GoodsIssueNote>("GoodsIssueNote_Delete",
                   param,
                   commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("OK"))
                {
                    resultService.Code = "0"; // Success
                    resultService.Message = "Deleted Successfully";
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

    public async Task<ResultService<GoodsIssueNote_Param>> SaveHeaderAndLine(GoodsIssueNote_Param entity)
    {
        var response = new ResultService<GoodsIssueNote_Param>();
        if (entity == null)
        {
            return new ResultService<GoodsIssueNote_Param>()
            {
                Code = "1",
                Message = "Entity is not valid"
            };
        }
        try
        {
            string Message = string.Empty;
            entity.GINs[0].GINCode = !entity.GINs[0].GINCode.Contains("GIN") ? string.Empty : entity.GINs[0].GINCode;

            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_Header", General.ConvertToDataTable(entity.GINs).AsTableValuedParameter("UDTT_GoodsIssueNoteHeader"));
                param.Add("@udtt_Detail", General.ConvertToDataTable(entity.GINLines).AsTableValuedParameter("UDTT_GoodsIssueNoteDetail"));
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<GoodsIssueNote>("GoodsIssueNote_Save",
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

    public async Task<ResultService<string>> DeleteLine(List<GoodsIssueNoteLine> entity)
    {
        ResultService<string> resultService = new();
        try
        {
            string Message = string.Empty;
           
            using (var connection = new SqlConnection(_dapperConnectionString))
            {

                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@udtt_Detail", General.ConvertToDataTable(entity).AsTableValuedParameter("UDTT_GoodsIssueNoteDetail"));
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<GoodsIssueNote>("GoodsIssueNoteDetail_Delete_Multi",
                   param,
                   commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("OK"))
                {
                    resultService.Code = "0"; // Success
                    resultService.Message = "Deleted Successfully";
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

    public async Task<ResultService<IEnumerable<GoodsIssueNoteLine>>> GetLineByRefCode(string ginCode)
    {
        var response = new ResultService<IEnumerable<GoodsIssueNoteLine>>();
      
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(_dapperConnectionString))
            {
                await sqlConnection.OpenAsync();
                var parameter = new DynamicParameters();
                parameter.Add("@GINCode", ginCode);
                var result = await sqlConnection.QueryAsync<GoodsIssueNoteLine>("GoodsIssueNoteDetail_GetByGINCode",
                    parameter,
                    commandType: CommandType.StoredProcedure,
                       commandTimeout: TimeoutInSeconds);

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

    public async Task<ResultService<string>> Delete_HeaderAndDetail(int ginID) //
    {
        ResultService<string> resultService = new();
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@ginID", ginID);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<GoodsIssueNote>("GoodsIssueNote_Delete_HeaderAndDetail",
                   param,
                   commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("OK"))
                {
                    resultService.Code = "0"; // Success
                    resultService.Message = "Deleted Successfully";
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
}


