using Base.BaseService;
using Base.ProductClassification;
using Context.ProductClassification;
using Context.ProductProperties;
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
public class ProductCategoryProvider : ICRUD_Service<ProductCategory, int>, IProductCategoryProvider
{
    private readonly DB_ProductClassification_Context _dB;
    private readonly IConfiguration _configuration;

    public ProductCategoryProvider(DB_ProductClassification_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<ProductCategory> Create(ProductCategory entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.ProductCategories.AddAsync(entity);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
    }

    public async Task<string> Delete(int id)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                ProductCategory obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.ProductCategories.Remove(obj);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return "true";
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return null;
            }
        }
    }

    public async Task<ProductCategory> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<ProductCategory>("ProductCategory_GetByID",
                new
                {
                    ID = id
                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<IEnumerable<ProductCategory>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<ProductCategory>("ProductCategory_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<ProductCategory> Update(ProductCategory entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.ProductCategories.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                obj.CategoryCode = entity.CategoryCode;
                obj.CategoryName = entity.CategoryName;
                obj.UpdatedBy = entity.UpdatedBy;
                obj.UpdatedDate = DateTime.Now;


                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
    }
}
