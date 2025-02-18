using Base.BaseService;
using Base.ProductProperties;
using Context.ProductProperties;
using Core.MasterData;
using Core.ProductProperties;
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
using System.Transactions;

namespace Servicer.ProductProperties;
public class ColorProvider : ICRUD_Service<Color, int>, IColorProvider
{
    private readonly DB_ProductProperties_Context _dB;
    private readonly IConfiguration _configuration;

    public ColorProvider(DB_ProductProperties_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<Color> Create(Color entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.Colors.AddAsync(entity);
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
                Color obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.Colors.Remove(obj);
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

    public async Task<Color> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<Color>("Color_GetByID",
                new
                {
                    ID = id
                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<IEnumerable<Color>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<Color>("Color_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<Color> Update(Color entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.Colors.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                obj.ColorCode = entity.ColorCode;
                obj.ColorName = entity.ColorName;
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
