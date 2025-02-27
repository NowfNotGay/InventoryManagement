using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.BaseClass;
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

    public async Task<ResultService<BusinessPartner>> Create(BusinessPartner entity)
    {
        ResultService<BusinessPartner> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                await _dB.BusinessPartners.AddAsync(entity);
                if( _dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to save data";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = entity;
                return result;
            }
            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();
                result.Code = "1";
                result.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return result;
            }
            catch (ArgumentException ex)
            {
                await transaction.RollbackAsync();
                result.Code = "2";
                result.Message = ex.Message;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }

        }
    }

    public async Task<ResultService<string>> Delete(int id)
    {
        ResultService<string> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await Get(id);
                if (!obj.Code.Equals("0"))
                {
                    result.Message = "Entity Not Found!";
                    result.Code = "-1";
                    result.Data = string.Empty;
                    return result;
                }
                _dB.BusinessPartners.Remove(obj.Data);
                if(_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to delete data";
                    result.Code = "-1";
                    result.Data = string.Empty;
                    return result;
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = string.Empty;
                return result;
            }
            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();
                result.Code = "1";
                result.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return result;
            }
            catch (ArgumentException ex)
            {
                await transaction.RollbackAsync();
                result.Code = "2";
                result.Message = ex.Message;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }
        }
    }

    public async Task<ResultService<BusinessPartner>> Get(int id)
    {
        ResultService<BusinessPartner> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<BusinessPartner>("BusinessPartner_GetByID",
                    new
                    {
                        ID = id
                    },
                     commandType: CommandType.StoredProcedure,
                     commandTimeout: 240);
                if(rs == null)
                {
                    result.Message = "Entity Not Found!";
                    result.Code = "-1";
                }
                else
                {
                    result.Message = "Success";
                    result.Code = "0";
                }
                result.Data = rs;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }
        }
    }

    public async Task<ResultService<IEnumerable<BusinessPartner>>> GetAll()
    {
        ResultService<IEnumerable<BusinessPartner>> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QueryAsync<BusinessPartner>("BusinessPartner_GetAll",
                    new
                    {

                    },
                     commandType: CommandType.StoredProcedure,
                     commandTimeout: 240);
                if (rs == null)
                {
                    result.Message = "Entity Not Found!";
                    result.Code = "-1";
                }
                else
                {
                    result.Message = "Success";
                    result.Code = "0";
                }
                result.Data = rs;
                return result;
            }
            catch (Exception ex)
            {
            result.Message = ex.Message;
            result.Code = "999";
            return result;
            }
        }
    }

    public async Task<ResultService<BusinessPartner>> Update(BusinessPartner entity)
    {
        ResultService<BusinessPartner> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {

                var obj = await _dB.BusinessPartners.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Entity Not Found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }

                obj.PartnerCode = entity.PartnerCode;
                obj.PartnerName = entity.PartnerName;
                obj.IsSupplier = entity.IsSupplier;
                obj.IsCustomer = entity.IsCustomer;
                obj.ContactInfo = entity.ContactInfo;
                obj.StatusID = entity.StatusID;
                obj.UpdatedBy = entity.UpdatedBy;
                obj.UpdatedDate = DateTime.Now;

                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to save data";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = entity;
                return result;
            }
            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();
                result.Code = "1";
                result.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return result;
            }
            catch (ArgumentException ex)
            {
                await transaction.RollbackAsync();
                result.Code = "2";
                result.Message = ex.Message;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }

        }
    }
}
