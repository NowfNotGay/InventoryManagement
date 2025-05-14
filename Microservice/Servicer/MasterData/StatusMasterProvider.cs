using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.BaseClass;
using Core.MasterData;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Servicer.MasterData;
public class StatusMasterProvider : ICRUD_Service<StatusMaster, int>, IStatusMasterProvider
{
    private readonly DB_MasterData_Context _dB;
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString;
    private const int TimeoutInSeconds = 240;

    public StatusMasterProvider(DB_MasterData_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER")!);

    }

    #region Nomal CRUD

    public async Task<ResultService<StatusMaster>> Create(StatusMaster entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            ResultService<StatusMaster> result = new();
            try
            {
                await _dB.StatusMasters.AddAsync(entity);
                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to create data";
                    result.Code = "1";
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = entity;
                return result;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                switch (ex.Number)
                {
                    case 53:
                        result.Code = "1001";
                        result.Message = "Database connection failed";
                        break;

                    case 208:
                        result.Code = "1007";
                        result.Message = "SQL Error: Table or column not found";
                        break;

                    case 156:
                        result.Code = "1005";
                        result.Message = "SQL syntax error";
                        break;

                    case 1205:
                        result.Code = "1006";
                        result.Message = "Deadlock occurred, transaction rolled back";
                        break;

                    default:
                        result.Code = "1099";
                        result.Message = $"SQL Error: {ex.Message}";
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null; // Optionally log the exception or handle it further
            }
        }
    }
    public async Task<ResultService<StatusMaster>> Update(StatusMaster entity)
    {
        ResultService<StatusMaster> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.StatusMasters.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "1";
                    result.Data = null;
                    return result;
                }

                obj.StatusCode = entity.StatusCode;
                obj.StatusName = entity.StatusName;
                obj.Description = entity.Description;
                obj.UpdatedDate = DateTime.Now;

                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to update data";
                    result.Code = "1";
                    result.Data = null;
                    return result;
                }

                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = entity;
                return result;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                switch (ex.Number)
                {
                    case 53:
                        result.Code = "1001";
                        result.Message = "Database connection failed";
                        break;

                    case 208:
                        result.Code = "1007";
                        result.Message = "SQL Error: Table or column not found";
                        break;

                    case 156:
                        result.Code = "1005";
                        result.Message = "SQL syntax error";
                        break;

                    case 1205:
                        result.Code = "1006";
                        result.Message = "Deadlock occurred, transaction rolled back";
                        break;

                    default:
                        result.Code = "1099";
                        result.Message = $"SQL Error: {ex.Message}";
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "1";
                return result;

            }
        }
    }

    public async Task<ResultService<string>> Delete(int id)
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
                _dB.StatusMasters.Remove(obj.Data);
                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to delete data";
                    result.Code = "1";
                    result.Data = "false";
                    return result;
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = "true";
                return result;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                switch (ex.Number)
                {
                    case 53:
                        result.Code = "1001";
                        result.Message = "Database connection failed";
                        break;

                    case 208:
                        result.Code = "1007";
                        result.Message = "SQL Error: Table or column not found";
                        break;

                    case 156:
                        result.Code = "1005";
                        result.Message = "SQL syntax error";
                        break;

                    case 1205:
                        result.Code = "1006";
                        result.Message = "Deadlock occurred, transaction rolled back";
                        break;

                    default:
                        result.Code = "1099";
                        result.Message = $"SQL Error: {ex.Message}";
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "1";
                return result;
            }
        }
    }

    #endregion

    #region Dapper CRUD
    public async Task<ResultService<StatusMaster>> Get(int id)
    {
        ResultService<StatusMaster> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString
            (_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            result.Data = await sqlconnect.QuerySingleOrDefaultAsync<StatusMaster>(
                "StatusMaster_GetByID",
                new
                {
                    ID = id
                },
                commandType: CommandType.StoredProcedure, // Fixed the error here
                commandTimeout: 240
                );
            if (result.Data == null)
            {
                result.Message = "Failed to get data";
                result.Code = "1";
            }
            else
            {
                result.Message = "Success";
                result.Code = "0";
            }
            return result;
        }
    }
 
    public async Task<ResultService<IEnumerable<StatusMaster>>> GetAll()
    {
        ResultService<IEnumerable<StatusMaster>> result = new();
        try
        {
            using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
            {
                await sqlconnect.OpenAsync();
                var data = await sqlconnect.QueryAsync<StatusMaster>(
                    "StatusMaster_GetAll",
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 240
                );

                result.Data = data.ToList();
                result.Message = result.Data.Any() ? "Success" : "No data found";
                result.Code = result.Data.Any() ? "0" : "1";
            }
        }
        catch (SqlException ex)
        {
            result.Message = $"SQL Error: {ex.Message}";
            result.Code = "1099";
        }
        catch (Exception ex)
        {
            result.Message = $"Error: {ex.Message}";
            result.Code = "1";
        }

        return result;
    }
    public async Task<ResultService<StatusMaster>> GetByCode(string statusCode)
    {
        ResultService<StatusMaster> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<StatusMaster>("StatusMaster_GetByCode",
                    new
                    {
                        StatusCode = statusCode
                    },
                     commandType: CommandType.StoredProcedure,
                     commandTimeout: 240);
                if (rs == null)
                {
                    result.Message = "Failed to get data";
                    result.Code = "1";
                }
                else
                {
                    result.Message = "Success";
                    result.Code = "0";
                }
                result.Data = rs;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }
        }
    }
    public async Task<ResultService<StatusMaster>> SaveByDapper(StatusMaster entity)
    {
        var response = new ResultService<StatusMaster>();

        if (entity == null)
        {
            return new ResultService<StatusMaster>()
            {
                Code = "1",
                Message = "Entity is not valid(BE)"
            };
        }
        try
        {
            string Message = string.Empty;
            entity.StatusCode = !entity.StatusCode.Contains("ST") ? string.Empty : entity.StatusCode;
            List<StatusMaster> lst = new List<StatusMaster>();
            entity.RowPointer = Guid.Empty;
            lst.Add(entity);

            DataTable dtHeader = General.ConvertToDataTable(lst);
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();

                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_StatusMaster", dtHeader.AsTableValuedParameter("UDTT_StatusMaster"));

                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<StatusMaster>("StatusMaster_Save",
                   param,
                   commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("successfully"))
                {
                    response.Code = "0"; // Success
                    response.Message = "Save Successfully(BE)";
                }
                else
                {
                    response.Code = "-999"; // Fail
                    response.Message = "Failed(BE)";
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
    public async Task<ResultService<string>> DeleteByDapper(string statusCode)
    {
        ResultService<string> resultService = new();
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {

                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@StatusCode", statusCode);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<StatusMaster>("StatusMaster_Delete",
                   param,
                   commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("OK"))
                {
                    resultService.Code = "0"; // Success
                    resultService.Message = "Deleted Successfully(BE)";
                }
                else
                {
                    resultService.Code = "-999";
                    resultService.Message = "Failed(BE)";
                    resultService.Message = resultMessage ?? "Failed(BE)"; // Sử dụng message từ stored procedure nếu có

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


    #endregion

}
