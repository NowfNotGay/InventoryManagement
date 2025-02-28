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
    public class ProductAttributeProvider : ICRUD_Service<ProductAttribute, int>, IProductAttributeProvider
    {
        private readonly DB_ProductManagement_Context _db;
        private readonly IConfiguration _configuration;

        public ProductAttributeProvider(DB_ProductManagement_Context db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<ResultService<ProductAttribute>> Create(ProductAttribute entity)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                ResultService<ProductAttribute> result = new();
                try
                {
                    await _db.ProductAttributes.AddAsync(entity);
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

                    _db.ProductAttributes.Remove(entity.Data);
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

        public async Task<ResultService<ProductAttribute>> Get(int id)
        {
            ResultService<ProductAttribute> result = new();
            using (var sqlConnection = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    var rs = await sqlConnection.QuerySingleOrDefaultAsync<ProductAttribute>("ProductAttribute_GetByID",
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

        public async Task<ResultService<IEnumerable<ProductAttribute>>> GetAll()
        {
            ResultService<IEnumerable<ProductAttribute>> result = new();
            using (var sqlConnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
            {
                try
                {
                    await sqlConnect.OpenAsync();
                    result.Data = await sqlConnect.QueryAsync<ProductAttribute>("ProductAttribute_GetAll",
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

        public async Task<ResultService<ProductAttribute>> Update(ProductAttribute entity)
        {
            ResultService<ProductAttribute> result = new();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var newObj = await _db.ProductAttributes.FindAsync(entity.RowPointer);
                    if (newObj == null)
                    {
                        result.Message = "Data not found!";
                        result.Code = "-1";
                        result.Data = null;
                        return result;
                    }
                    newObj.ProductID = entity.ProductID;
                    newObj.ColorID = entity.ColorID;
                    newObj.MaterialID = entity.MaterialID;
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
