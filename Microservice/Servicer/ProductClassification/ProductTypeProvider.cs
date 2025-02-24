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

    public async Task<ProductType> Create(ProductType entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.ProductTypes.AddAsync(entity);
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
                ProductType obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.ProductTypes.Remove(obj);
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

    public async Task<ProductType> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<ProductType>("ProductType_GetByID",
                new
                {
                    ID = id
                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<IEnumerable<ProductType>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<ProductType>("ProductType_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<ProductType> Update(ProductType entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.ProductTypes.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                obj.ProductTypeCode = entity.ProductTypeCode;
                obj.ProductTypeName = entity.ProductTypeName;
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

    Task<ResultService<ProductType>> ICRUD_Service<ProductType, int>.Create(ProductType entity)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<string>> ICRUD_Service<ProductType, int>.Delete(int id)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<ProductType>> ICRUD_Service<ProductType, int>.Get(int id)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<IEnumerable<ProductType>>> ICRUD_Service<ProductType, int>.GetAll()
    {
        throw new NotImplementedException();
    }

    Task<ResultService<ProductType>> ICRUD_Service<ProductType, int>.Update(ProductType entity)
    {
        throw new NotImplementedException();
    }
}
