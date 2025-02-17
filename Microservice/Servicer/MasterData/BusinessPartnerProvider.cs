using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.MasterData;
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
                transaction.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
             
                return null;
            }
        }
    }

    public Task<string> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessPartner> Get(int id)
    {
        throw new NotImplementedException();
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

    public Task<BusinessPartner> Update(BusinessPartner entity)
    {
        throw new NotImplementedException();
    }
}
