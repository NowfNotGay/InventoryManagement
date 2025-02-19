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
public class VehicleModelProvider : ICRUD_Service<VehicleModel, int>, IVehicleModelProvider
{
    private readonly DB_ProductClassification_Context _dB;
    private readonly IConfiguration _configuration;

    public VehicleModelProvider(DB_ProductClassification_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<VehicleModel> Create(VehicleModel entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.VehicleModels.AddAsync(entity);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw; // Ensuring any exceptions are thrown back to the caller
            }
        }
    }

    public async Task<string> Delete(int id)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                VehicleModel obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.VehicleModels.Remove(obj);
                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return "true";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<VehicleModel> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await 
                sqlconnect.QuerySingleOrDefaultAsync<VehicleModel>("VehicleModel_GetByID",
                new
                {
                    ID = id
                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }
    public async Task<IEnumerable<VehicleModel>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<VehicleModel>("VehicleModel_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }


    public async Task<VehicleModel> Update(VehicleModel entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.VehicleModels.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                // Update properties as necessary
                obj.ModelCode = entity.ModelCode;
                obj.ModelName = entity.ModelName;
                obj.BrandID = entity.BrandID;
                obj.UpdatedBy = entity.UpdatedBy;
                obj.UpdatedDate = DateTime.Now;

                await _dB.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw; // Rethrow the exception
            }
        }
    }
}


