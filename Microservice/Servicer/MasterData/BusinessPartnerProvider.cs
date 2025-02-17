using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.MasterData;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Servicer.MasterData;
public class BusinessPartnerProvider : ICRUD_Service<BusinessPartner, int>, IBusinessPartnerProvider
{
    private readonly DB_MasterData_Context _dB;
    private readonly IConfiguration _configuration;

    public BusinessPartnerProvider(DB_MasterData_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<BusinessPartner> Create(BusinessPartner entity)
    {
       using(var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.BusinessPartners.AddAsync(entity);
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
                BusinessPartner obj = await Get(id);
                if (obj == null)
                {
                    return null;
                }
                _dB.BusinessPartners.Remove(obj);
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

    public async Task<BusinessPartner> Get(int id)
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QuerySingleOrDefaultAsync<BusinessPartner>("BusinessPartner_GetByID",
                new
                {
                    ID = id
                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<IEnumerable<BusinessPartner>> GetAll()
    {
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            var rs = await sqlconnect.QueryAsync<BusinessPartner>("BusinessPartner_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);

            return rs;
        }
    }

    public async Task<BusinessPartner> Update(BusinessPartner entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.BusinessPartners.FindAsync(entity.RowPointer);
                if (obj == null) return null;

                obj.PartnerCode = entity.PartnerCode;
                obj.PartnerName = entity.PartnerName;
                obj.IsSupplier = entity.IsSupplier;
                obj.IsCustomer = entity.IsCustomer;
                obj.ContactInfo = entity.ContactInfo;
                obj.StatusID = entity.StatusID;
                obj.UpdatedDate = DateTime.Now;
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
