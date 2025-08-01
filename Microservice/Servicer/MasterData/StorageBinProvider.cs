using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.BaseClass;
using Core.MasterData;
using Core.MasterData.ProductProperties;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Dapper.SqlMapper;

namespace Servicer.MasterData;
public class StorageBinProvider : ICRUD_Service<StorageBin, int>, IStorageBinProvider
{
    private readonly DB_MasterData_Context _db;
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString = string.Empty;
    private const int TimeoutInSeconds = 240;
    public StorageBinProvider(DB_MasterData_Context db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER")!);
    }

    public async Task<ResultService<StorageBin>> Create(StorageBin entity)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            ResultService<StorageBin> result = new();
            try
            {
                await _db.StorageBins.AddAsync(entity);
                if (_db.SaveChanges() <= 0)
                {
                    result.Message = "Failed to create data";
                    result.Code = "-1";
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = entity;
                return result;
            }
            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();
                result.Code = "1";
                result.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
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

    public async Task<ResultService<string>> Delete(int id)
    {
        ResultService<string> result = new();
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var entity = await Get(id);
                if (!entity.Code.Equals("0"))
                {
                    result.Message = entity.Message;
                    result.Code = entity.Code;
                    result.Data = "false";
                    return result;
                }

                _db.StorageBins.Remove(entity.Data);
                if (_db.SaveChanges() <= 0)
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
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }
        }
    }

    public async Task<ResultService<StorageBin>> Get(int id)
    {
        ResultService<StorageBin> result = new();
        using (var sqlConnection = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlConnection.OpenAsync();
                var rs = await sqlConnection.QuerySingleOrDefaultAsync<StorageBin>("StorageBin_GetByID",
                    new
                    {
                        ID = id
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
                    result.Data = rs;
                }

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

    public async Task<ResultService<IEnumerable<StorageBin>>> GetAll()
    {
        ResultService<IEnumerable<StorageBin>> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<StorageBin>("StorageBin_GetAll",
                    new
                    {

                    },
                     commandType: CommandType.StoredProcedure,
                     commandTimeout: 240);
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
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }
        }
    }

    public async Task<ResultService<StorageBin>> Update(StorageBin entity)
    {
        ResultService<StorageBin> result = new();
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var newObj = await _db.StorageBins.FindAsync(entity.RowPointer);
                if (newObj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }
                newObj.StorageBinCode = entity.StorageBinCode;
                newObj.Description = entity.Description;
                newObj.WarehouseCode = entity.WarehouseCode;
                newObj.UpdatedDate = DateTime.Now;
                newObj.UpdatedBy = entity.UpdatedBy;



                if (_db.SaveChanges() <= 0)
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
            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();
                result.Code = "1";
                result.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
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





    #region DAPPER CRUD
    public async Task<ResultService<StorageBin>> SaveByDapper(StorageBin entity)
    {
        ResultService<StorageBin> result = new();
        if (entity == null)
        {
            result.Code = "-1";
            result.Data = null;
            return result;
        }
        try
        {
            string Message = string.Empty;
            entity.RowPointer = Guid.Empty;
            entity.StorageBinCode = !entity.StorageBinCode.Contains("MT") ? string.Empty : entity.StorageBinCode;
            List<StorageBin> list = new();
            list.Add(entity);
            DataTable data = General.ConvertToDataTable(list);

            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_StorageBin", data.AsTableValuedParameter("UDTT_StorageBin"));
                param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                var resultData = (await connection.QueryAsync<StorageBin>("StorageBin_Save",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: TimeoutInSeconds)).FirstOrDefault();
                var resultMessage = param.Get<string>("@Message");
                if (resultMessage.Contains("successfully"))
                {
                    result.Code = "0";
                    result.Message = "Save Successfully";

                    result.Data = (StorageBin)resultData!;
                }
                else
                {
                    result.Code = "-1";
                    result.Message = "Failed";
                }
                return result;
            }
        }
        catch (SqlException sqlex)
        {

            result.Code = "2";
            result.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {

            result.Code = "3";
            result.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (DbUpdateException ex)
        {

            result.Code = "4";
            result.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (OperationCanceledException ex)
        {

            result.Code = "5";
            result.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
            return result;
        }
    }


    public async Task<ResultService<string>> DeleteByDapper(string code)
    {
        ResultService<string> result = new();
        if (string.IsNullOrEmpty(code))
        {
            result.Code = "-1";
            result.Message = "Code is null";
            return result;
        }
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@MaterialCode", code);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<string>("StorageBin_Delete", param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
                //Output Message từ Procedure
                var resultMessage = param.Get<string>("@Message");
                if (resultMessage.ToLower().Contains("successfully"))
                {
                    result.Code = "0";
                    result.Message = resultMessage;
                    result.Data = code;
                }
                else
                {
                    result.Code = "-1";
                    result.Message = resultMessage;
                    result.Data = string.Empty;
                }
                return result;
            }
        }
        catch (SqlException sqlex)
        {
            result.Code = "2";
            result.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            result.Code = "3";
            result.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (DbUpdateException ex)
        {
            result.Code = "4";
            result.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (OperationCanceledException ex)
        {
            result.Code = "5";
            result.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
            return result;
        }

    }

    public async Task<ResultService<IEnumerable<StorageBin>>> GetAllByWarehouseCode(string code)
    {
        ResultService<IEnumerable<StorageBin>> result = new();
        using (var sqlConnection = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlConnection.OpenAsync();
                var rs = await sqlConnection.QueryAsync<StorageBin>("StorageBins_GetByWarehouse",
                    new
                    {
                        WarehouseCode = code
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
                    result.Data = rs;
                }

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


    #endregion
}
