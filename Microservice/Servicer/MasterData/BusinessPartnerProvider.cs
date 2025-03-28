using Azure;
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
    private readonly string _dapperConnectionString = string.Empty;
    private const int TimeoutInSeconds = 240;

    public BusinessPartnerProvider(DB_MasterData_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"));
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

    #region DAPPER CRUD
    public async Task<ResultService<BusinessPartner>> SaveByDapper(BusinessPartner entity)
    {
        ResultService<BusinessPartner> result = new();
        if (entity == null)
        {
            result.Code = "-1";
            result.Message = "Entity is null";
            return result;
        }

        try
        {
            string Message = string.Empty;
            entity.PartnerCode = !entity.PartnerCode.Contains("BP") ? string.Empty : entity.PartnerCode;
            List<BusinessPartner> list = new List<BusinessPartner>();
            entity.RowPointer = Guid.Empty;
            list.Add(entity);
            DataTable data = General.ConvertToDataTable(list);
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_BusinessPartner", data.AsTableValuedParameter("UDTT_BusinessPartner"));
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<BusinessPartner>("BusinessPartner_Save", param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
                //Output Message từ Procedure
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("successfully"))
                {
                    result.Code = "0";
                    result.Message = resultMessage;
                    result.Data = entity;

                }
                else
                {
                    result.Code = "-1";
                    result.Message = resultMessage;
                    result.Data = null;

                }
                return result;
            }
        }
        catch (SqlException sqlex)
        {

            result.Code = "2";
            result.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {

            result.Code = "3";
            result.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (DbUpdateException ex)
        {

            result.Code = "4";
            result.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (OperationCanceledException ex)
        {

            result.Code = "5";
            result.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (Exception ex)
        {

            result.Code = "6";
            result.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
            return result;
        }
    }

    public async Task<ResultService<string>> DeleteByDapper(string partnerCode)
    {
        ResultService<string> result = new();
        if (string.IsNullOrEmpty(partnerCode))
        {
            result.Code = "-1";
            result.Message = "Entity is null";
            return result;
        }
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@PartnerCode", partnerCode);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<string>("BusinessPartner_Delete", param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
                //Output Message từ Procedure
                var resultMessage = param.Get<string>("@Message");
                if (resultMessage.ToLower().Contains("successfully"))
                {
                    result.Code = "0";
                    result.Message = resultMessage;
                    result.Data = partnerCode;
                }
                else
                {
                    result.Code = "-1";
                    result.Message = resultMessage;
                    result.Data = string.Empty;
                }
                return result;
            }
        }
        catch (SqlException sqlex)
        {
            result.Code = "2";
            result.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            result.Code = "3";
            result.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (DbUpdateException ex)
        {
            result.Code = "4";
            result.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (OperationCanceledException ex)
        {
            result.Code = "5";
            result.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
            return result;
        }
    }

    #endregion
}
