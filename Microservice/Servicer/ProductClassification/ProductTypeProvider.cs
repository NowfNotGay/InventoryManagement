using Base.BaseService;
using Base.ProductClassification;
using Context.ProductClassification;
using Core.BaseClass;
using Core.ProductClassification;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicer.ProductClassification;
public class ProductTypeProvider : ICRUD_Service<ProductType, int>, IProductTypeProvider
{
    private readonly DB_ProductClassification_Context _dB;
    private readonly IConfiguration _configuration;

    public ProductTypeProvider(DB_ProductClassification_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<ResultService<ProductType>> Create(ProductType entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            ResultService<ProductType> result = new();
            try
            {
                await _dB.ProductTypes.AddAsync(entity);


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
                _dB.ProductTypes.Remove(obj.Data);
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

    public async Task<ResultService<ProductType>> Get(int id)
    {
        ResultService<ProductType> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<ProductType>("ProductType_GetByID",
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

    public async Task<ResultService<IEnumerable<ProductType>>> GetAll()
    {
        ResultService<IEnumerable<ProductType>> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QueryAsync<ProductType>("ProductType_GetAll",
                    new
                    {

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

    public async Task<ResultService<ProductType>> Update(ProductType entity)
    {
        ResultService<ProductType> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.ProductTypes.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }
                obj.ProductTypeCode = entity.ProductTypeCode;
                obj.ProductTypeName = entity.ProductTypeName;
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
}
