using Base.BaseService;
using Base.ProductManagement;
using Core.BaseClass;
using Core.ProductManagement;
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

namespace Servicer.MasterData.ProductManagement;
public class ProductProvider : ICRUD_Service<Product, int>, IProductProvider
{
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString = string.Empty;
    private const int TimeoutInSeconds = 240;

    public ProductProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"));
    }

    public Task<ResultService<Product>> Create(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<string>> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultService<Product>> Get(int id)
    {
        ResultService<Product> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<Product>("Product_GetByID",
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

    public async Task<ResultService<ProductParam>> GetByIDParam(int id)
    {
        ResultService<ProductParam> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<ProductParam>("Product_GetByID_Param",
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

    public async Task<ResultService<IEnumerable<Product>>> GetAll()
    {
        ResultService<IEnumerable<Product>> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<Product>("Product_GetAll",
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

    public async Task<ResultService<IEnumerable<ProductParam>>> GetAllProductParam()
    {
        ResultService<IEnumerable<ProductParam>> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<ProductParam>("Product_GetAll_Param",
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

    public Task<ResultService<Product>> Update(Product entity)
    {
        throw new NotImplementedException();
    }


    #region DAPPER CRUD
    public async Task<ResultService<Product>> SaveByDapper(Product entity)
    {
        ResultService<Product> result = new();
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
            entity.ProductCode = !entity.ProductCode.Contains("PRO") ? string.Empty : entity.ProductCode;
            List<Product> list = new();
            list.Add(entity);
            DataTable data = General.ConvertToDataTable(list);

            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_Product", data.AsTableValuedParameter("UDTT_Product"));
                param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                await connection.QueryAsync<Product>("Product_Save",
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
            result.Message = "Entity is null";
            return result;
        }
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@ProductCode", code);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<string>("Product_Delete", param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
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
