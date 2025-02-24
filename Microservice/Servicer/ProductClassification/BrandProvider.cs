using Base.BaseService;
using Base.ProductClassification;
using Context.ProductClassification;
using Context.ProductProperties;
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

public class BrandProvider : ICRUD_Service<Brand, int>, IBrandProvider
{
    private readonly DB_ProductClassification_Context _dB;
    private readonly IConfiguration _configuration;

    public BrandProvider(DB_ProductClassification_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<Brand> Create(Brand entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.Brands.AddAsync(entity);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
//                throw; // Rethrow exception after rollback
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
                Brand obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.Brands.Remove(obj);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return "true";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
//                throw; // Rethrow exception after rollback
                return null;

            }
        }
    }

    public async Task<Brand> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<Brand>("Brand_GetByID",
                new
                {
                    ID = id
                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<IEnumerable<Brand>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<Brand>("Brand_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<Brand> Update(Brand entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.Brands.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                obj.BrandCode = entity.BrandCode; 
                obj.BrandName = entity.BrandName; 
                obj.UpdatedBy = entity.UpdatedBy;
                obj.UpdatedDate = DateTime.Now;

                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //throw; // Rethrow exception after rollback
                return null;

            }
        }
    }

    Task<ResultService<Brand>> ICRUD_Service<Brand, int>.Create(Brand entity)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<string>> ICRUD_Service<Brand, int>.Delete(int id)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<Brand>> ICRUD_Service<Brand, int>.Get(int id)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<IEnumerable<Brand>>> ICRUD_Service<Brand, int>.GetAll()
    {
        throw new NotImplementedException();
    }

    Task<ResultService<Brand>> ICRUD_Service<Brand, int>.Update(Brand entity)
    {
        throw new NotImplementedException();
    }
}
