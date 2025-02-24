using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.BaseClass;
using Core.MasterData;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Dapper.SqlMapper;

namespace Servicer.MasterData;
public class StorageBinProvider : ICRUD_Service<StorageBin, int>, IStorageBinProvider
{
    private readonly DB_MasterData_Context _db;
    private readonly IConfiguration _configuration;

    public StorageBinProvider(DB_MasterData_Context db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<StorageBin> Create(StorageBin entity)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                await _db.StorageBins.AddAsync(entity);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null;
            }
        }
    }

    public async Task<string> Delete(int id)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                StorageBin obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _db.StorageBins.Remove(obj);
                await _db.SaveChangesAsync();
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

    public async Task<StorageBin> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<StorageBin>("StorageBin_GetByID",
                new
                {
                    ID = id
                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<IEnumerable<StorageBin>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<StorageBin>("StorageBin_GetAll",
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<StorageBin> Update(StorageBin entity)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var obj = await _db.StorageBins.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                obj.WareHouseID = entity.WareHouseID;
                obj.StorageBinCode = entity.StorageBinCode;
                obj.Description = entity.Description;
                obj.UpdatedDate = entity.UpdatedDate;
                obj.CreatedDate = entity.CreatedDate;

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null;
            }
        }
    }

    Task<ResultService<StorageBin>> ICRUD_Service<StorageBin, int>.Create(StorageBin entity)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<string>> ICRUD_Service<StorageBin, int>.Delete(int id)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<StorageBin>> ICRUD_Service<StorageBin, int>.Get(int id)
    {
        throw new NotImplementedException();
    }

    Task<ResultService<IEnumerable<StorageBin>>> ICRUD_Service<StorageBin, int>.GetAll()
    {
        throw new NotImplementedException();
    }

    Task<ResultService<StorageBin>> ICRUD_Service<StorageBin, int>.Update(StorageBin entity)
    {
        throw new NotImplementedException();
    }
}
