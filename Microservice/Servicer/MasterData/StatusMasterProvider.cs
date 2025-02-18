using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.MasterData;
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

namespace Servicer.MasterData;
public class StatusMasterProvider : ICRUD_Service<StatusMaster, int>, IStatusMasterProvider
{
    private readonly DB_MasterData_Context _dB;
    private readonly IConfiguration _configuration;

    public StatusMasterProvider(DB_MasterData_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<StatusMaster> Create(StatusMaster entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.StatusMasters.AddAsync(entity);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null; // Optionally log the exception or handle it further
            }
        }
    }

    public async Task<string> Delete(int id)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                StatusMaster obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.StatusMasters.Remove(obj);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Deleted successfully";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null; // Optionally log the exception or handle it further
            }
        }
    }

    public async Task<StatusMaster> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString
            (_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<StatusMaster>(
                "StatusMaster_GetByID",
                new
                {
                    ID = id
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 240
                );
            return rs;
        }

    }

    public async Task<IEnumerable<StatusMaster>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString
            (_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<StatusMaster>(
                "StatusMaster_GetAll",
                commandType: CommandType.StoredProcedure,
                commandTimeout: 240
                );
            return rs;
        }
    }

    public async Task<StatusMaster> Update(StatusMaster entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.StatusMasters.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                obj.StatusCode = entity.StatusCode;
                obj.StatusName = entity.StatusName;
                obj.Description = entity.Description;
                obj.UpdatedDate = DateTime.Now;
         

                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null; // Optionally log the exception or handle it further
            }
        }
    }
}
