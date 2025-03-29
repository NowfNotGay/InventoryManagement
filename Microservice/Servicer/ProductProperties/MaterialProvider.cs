using Base.BaseService;
using Base.ProductProperties;
using Context.ProductProperties;
using Core.BaseClass;
using Core.ProductProperties;
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

namespace Servicer.ProductProperties;
public class MaterialProvider : ICRUD_Service<Material, int>, IMaterialProvider
{
    private readonly DB_ProductProperties_Context _dB;
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString = string.Empty;
    private const int TimeoutInSeconds = 240;

    public MaterialProvider(DB_ProductProperties_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER")!);
    }

    public async Task<ResultService<Material>> Create(Material entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            ResultService<Material> result = new();
            try
            {
                await _dB.Materials.AddAsync(entity);


                if (_dB.SaveChanges() <= 0)
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
            catch (ArgumentException ex)
            {
                await transaction.RollbackAsync();
                result.Code = "2";
                result.Message = ex.Message;
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
                _dB.Materials.Remove(obj.Data);
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
                result.Message = ex.Message;
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

    public async Task<ResultService<Material>> Get(int id)
    {
        ResultService<Material> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<Material>("Material_GetByID",
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

    public async Task<ResultService<IEnumerable<Material>>> GetAll()
    {
        ResultService<IEnumerable<Material>> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<Material>("Material_GetAll",
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

    public async Task<ResultService<Material>> Update(Material entity)
    {
        ResultService<Material> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.Materials.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }
                obj.MaterialCode = entity.MaterialCode;
                obj.MaterialName = entity.MaterialName;
                obj.UpdatedBy = entity.UpdatedBy;
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
                result.Message = ex.Message;
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
    public async Task<ResultService<Material>> SaveByDapper(Material entity)
    {
        ResultService<Material> result = new();
        if (entity == null)
        {
            result.Code = "-1";
            result.Data = null;
            return result;
        }
        try
        {
            string Message = String.Empty;
            entity.RowPointer = Guid.Empty;
            entity.MaterialCode = !entity.MaterialCode.Contains("MT") ? string.Empty : entity.MaterialCode;
            List<Material> list = new();
            list.Add(entity);
            DataTable data = General.ConvertToDataTable(list);

            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_Material", data.AsTableValuedParameter("UDTT_Material"));
                param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                await connection.QueryAsync<Material>("Material_Save",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");
                if (resultMessage.Contains("successfully"))
                {
                    result.Code = "0";
                    result.Message = "Save Successfully";
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
                await connection.QueryAsync<string>("Material_Delete", param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
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


    #endregion
}
