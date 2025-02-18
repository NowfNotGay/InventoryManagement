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
using static Dapper.SqlMapper;

namespace Servicer.MasterData;
public class WarehouseProvider : ICRUD_Service<Warehouse, int>,
    IWarehouseProvider
{
    private readonly DB_MasterData_Context _dB;
    private readonly IConfiguration _configuration;
    public WarehouseProvider(DB_MasterData_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }
    public async Task<Warehouse> Create(Warehouse entity)
    {
        using(var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.Warehouses.AddAsync(entity);
                await _dB.SaveChangesAsync();
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
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                Warehouse obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.Warehouses.Remove(obj);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Deleted Successfully";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null;
            }
        }
    }
    public async Task<Warehouse> Get(int id)
    {
        using(var sqlconnect = new SqlConnection(General.DecryptString
                (_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<Warehouse>( 
                "Warehouse_GetByID",
                new 
                { 
                    ID = id
                }, 
                commandType: CommandType.StoredProcedure,
                commandTimeout: 240);
            return rs;


        }
    }
    public async Task<IEnumerable<Warehouse>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<Warehouse>("Warehouse_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<Warehouse> Update(Warehouse entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                // Find the warehouse by ID
                var obj = new Warehouse();
                if (entity.RowPointer != null)
                {
                 obj = await _dB.Warehouses.FindAsync(entity.RowPointer);

                if (obj == null) return null; 
                }

                obj.WarehouseCode = entity.WarehouseCode;
                obj.WarehouseName = entity.WarehouseName;
                obj.AllowNegativeStock = entity.AllowNegativeStock;
                obj.Address = entity.Address;
                obj.BinLocationCount = entity.BinLocationCount;
                obj.UpdatedDate = DateTime.Now;  
                obj.UpdatedBy = entity.UpdatedBy; 

                await _dB.SaveChangesAsync();
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

}

