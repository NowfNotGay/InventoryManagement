using Base.BaseService;
using Base.ProductManagement;
using Context.ProductManagement;
using Core.BaseClass;
using Core.ProductManagement;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Servicer.ProductManagement
{
    public class ProductVariantProvider : ICRUD_Service<ProductVariant, int>, IProductVariantProvider
    {
        private readonly DB_ProductManagement_Context _db;
        private readonly IConfiguration _configuration;

        public ProductVariantProvider(DB_ProductManagement_Context db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<ResultService<ProductVariant>> Create(ProductVariant entity)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                ResultService<ProductVariant> result = new();
                try
                {
                    await _db.ProductVariants.AddAsync(entity);
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
                    result.Message = ex.InnerException?.Message ?? ex.Message;
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

                    _db.ProductVariants.Remove(entity.Data);
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

        public async Task<ResultService<ProductVariant>> Get(int id)
        {
            ResultService<ProductVariant> result = new();
            using (var sqlConnection = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    var rs = await sqlConnection.QuerySingleOrDefaultAsync<ProductVariant>("ProductVariant_GetByID",
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

        public async Task<ResultService<IEnumerable<ProductVariant>>> GetAll()
        {
            ResultService<IEnumerable<ProductVariant>> result = new();
            using (var sqlConnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
            {
                try
                {
                    await sqlConnect.OpenAsync();
                    result.Data = await sqlConnect.QueryAsync<ProductVariant>("ProductVariant_GetAll",
                      commandType: System.Data.CommandType.StoredProcedure,
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

        public async Task<ResultService<ProductVariant>> Update(ProductVariant entity)
        {
            ResultService<ProductVariant> result = new();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var newObj = await _db.ProductVariants.FindAsync(entity.RowPointer);
                    if (newObj == null)
                    {
                        result.Message = "Data not found!";
                        result.Code = "-1";
                        result.Data = null;
                        return result;
                    }
                    newObj.ProductID = entity.ProductID;
                    newObj.Attributes = entity.Attributes;
                    newObj.ProductVariantCode = entity.ProductVariantCode;
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
    }
}
