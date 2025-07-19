using Base.BaseService;
using Base.ProductManagement;
using Context.MasterData.ProductManagement;
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
using System.Threading.Tasks;

namespace Servicer.MasterData.ProductManagement;

public class ProductUoMConversionProvider : ICRUD_Service<ProductUoMConversion, int>, IProductUoMConversionProvider
{
    private readonly DB_ProductManagement_Context _dB;
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString;
    private const int TimeoutInSeconds = 240;

    public ProductUoMConversionProvider(DB_ProductManagement_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER")!);
    }

    #region Normal CRUD

    public async Task<ResultService<ProductUoMConversion>> Create(ProductUoMConversion entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            ResultService<ProductUoMConversion> result = new();
            try
            {
                await _dB.ProductUoMConversions.AddAsync(entity);

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
                result.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration. Details: {ex.GetType()} - {ex.Message}";
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
                _dB.ProductUoMConversions.Remove(obj.Data);
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
                result.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration. Details: {ex.GetType()} - {ex.Message}";
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

    public async Task<ResultService<ProductUoMConversion>> Update(ProductUoMConversion entity)
    {
        ResultService<ProductUoMConversion> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.ProductUoMConversions.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }

                obj.ProductUoMConversionCode = entity.ProductUoMConversionCode;
                obj.ProductCode = entity.ProductCode;
                obj.FromUoMID = entity.FromUoMID;
                obj.ToUoMID = entity.ToUoMID;
                obj.ConversionRate = entity.ConversionRate;
                obj.UpdatedDate = DateTime.Now;
                obj.UpdatedBy = entity.UpdatedBy;

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
                result.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration. Details: {ex.GetType()} - {ex.Message}";
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

    #endregion

    #region Dapper CRUD

    public async Task<ResultService<ProductUoMConversion>> Get(int id)
    {
        ResultService<ProductUoMConversion> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<ProductUoMConversion>(
                    "ProductUoMConversion_GetByID",
                    new { ID = id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: TimeoutInSeconds);
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

    public async Task<ResultService<IEnumerable<ProductUoMConversion>>> GetAll()
    {
        ResultService<IEnumerable<ProductUoMConversion>> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<ProductUoMConversion>(
                    "ProductUoMConversion_GetAll",
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: TimeoutInSeconds);

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

    public async Task<ResultService<ProductUoMConversion>> GetByCode(string productUoMConversionCode)
    {
        ResultService<ProductUoMConversion> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<ProductUoMConversion>(
                    "ProductUoMConversion_GetByCode",
                    new { ProductUoMConversionCode = productUoMConversionCode },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: TimeoutInSeconds);
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

    public async Task<ResultService<ProductUoMConversion>> SaveByDapper(ProductUoMConversion entity)
    {
        var response = new ResultService<ProductUoMConversion>();

        if (entity == null)
        {
            return new ResultService<ProductUoMConversion>()
            {
                Code = "1",
                Message = "Entity is not valid(BE)"
            };
        }
        try
        {
            string Message = string.Empty;
            // Kiểm tra tiền tố của ProductUoMConversionCode (giả sử tiền tố là "UoMConv")
            entity.ProductUoMConversionCode = !entity.ProductUoMConversionCode.Contains("PUOMC") ? string.Empty : entity.ProductUoMConversionCode;
            List<ProductUoMConversion> lst = new List<ProductUoMConversion>();
            entity.RowPointer = Guid.Empty;
            lst.Add(entity);

            DataTable dtHeader = General.ConvertToDataTable(lst);
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();

                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_ProductUoMConversion", dtHeader.AsTableValuedParameter("UDTT_ProductUoMConversion"));
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                var result = (await connection.QueryAsync<ProductUoMConversion>(
                    "ProductUoMConversion_Save",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: TimeoutInSeconds)).FirstOrDefault();

                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("successfully"))
                {
                    response.Code = "0"; // Success
                    response.Data = (ProductUoMConversion) result;
                    response.Message = "Save Successfully(BE)";
                }
                else
                {
                    response.Code = "-999"; // Fail
                    response.Message = resultMessage != null ? $"Failed: {resultMessage}" : "Failed(BE)";
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
            response.Message = $"Concurrency error or Conflict happened: {ex.GetType()} - {ex.Message}";
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

    public async Task<ResultService<string>> DeleteByDapper(string productUoMConversionCode)
    {
        ResultService<string> resultService = new();
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@ProductUoMConversionCode", productUoMConversionCode);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                await connection.QueryAsync<ProductUoMConversion>(
                    "ProductUoMConversion_Delete",
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
                    resultService.Message = resultMessage != null ? $"Failed: {resultMessage}" : "Failed(BE)";
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