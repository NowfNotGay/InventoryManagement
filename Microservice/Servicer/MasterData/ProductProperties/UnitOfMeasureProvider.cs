using Azure;
using Base.BaseService;
using Base.MasterData.ProductProperties;
using Context.MasterData.ProductProperties;
using Core.BaseClass;
using Core.MasterData;
using Core.MasterData.ProductClassification;
using Core.MasterData.ProductProperties;
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
using System.Transactions;
using static Dapper.SqlMapper;

namespace Servicer.MasterData.ProductProperties;
public class UnitOfMeasureProvider : ICRUD_Service<UnitOfMeasure, int>, IUnitOfMeasureProvider
{
    private readonly DB_ProductProperties_Context _dB;
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString;
    private const int TimeoutInSeconds = 240;

    public UnitOfMeasureProvider(DB_ProductProperties_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER")!);
    }

    public async Task<ResultService<UnitOfMeasure>> Create(UnitOfMeasure entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            ResultService<UnitOfMeasure> result = new();
            try
            {
                await _dB.UnitOfMeasures.AddAsync(entity);

                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to create data";
                    result.Code = "-1";
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
                    result.Message = obj.Message;
                    result.Code = obj.Code;
                    result.Data = "false";
                    return result;

                }

                _dB.UnitOfMeasures.Remove(obj.Data);
                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to delete data";
                    result.Code = "-1";
                    result.Data = "false";
                    return result;
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = "true";
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

    public async Task<ResultService<UnitOfMeasure>> Update(UnitOfMeasure entity)
    {
        ResultService<UnitOfMeasure> result = new();

        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.UnitOfMeasures.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }

                // Update properties
                obj.UoMCode = entity.UoMCode;
                obj.UoMName = entity.UoMName;
                obj.UoMDescription = entity.UoMDescription;
                obj.UpdatedBy = entity.UpdatedBy;
                obj.UpdatedDate = DateTime.Now;
                if (_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to update data";
                    result.Code = "1";
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
    #region Dapper CRUD

    public async Task<ResultService<UnitOfMeasure>> Get(int id)
    {
        ResultService<UnitOfMeasure> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<UnitOfMeasure>("UoM_GetByID",
                    new
                    {
                        ID = id
                    },
                     commandType: CommandType.StoredProcedure,
                     commandTimeout: 240);
                if (rs == null)
                {
                    result.Message = "Failed to get data";
                    result.Code = "1";

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

    public async Task<ResultService<IEnumerable<UnitOfMeasure>>> GetAll()
    {
        ResultService<IEnumerable<UnitOfMeasure>> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<UnitOfMeasure>("UoM_GetAll",

                         commandType: CommandType.StoredProcedure,
                         commandTimeout: 240);
                if (result.Data == null)
                {
                    result.Message = "Failed to get data";
                    result.Code = "1";
                }
                else
                {
                    result.Message = "Success";
                    result.Code = "0";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }
    }

    public async Task<ResultService<UnitOfMeasure>> SaveByDapper(UnitOfMeasure entity)
    {
        var response = new ResultService<UnitOfMeasure>();

        if (entity == null)
        {
            return new ResultService<UnitOfMeasure>()
            {
                Code = "1",
                Message = "Entity is not valid(BE)"
            };
        }
        try
        {
            string Message = string.Empty;
            entity.UoMCode = !entity.UoMCode.Contains("UOM") ? string.Empty : entity.UoMCode;
            List<UnitOfMeasure> lst = new List<UnitOfMeasure>();
            entity.RowPointer = Guid.Empty;
            lst.Add(entity);

            DataTable dtHeader = General.ConvertToDataTable(lst);
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();

                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_UnitOfMeasure", dtHeader.AsTableValuedParameter("UDTT_UnitOfMeasure"));

                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                var resultData =  (await connection.QueryAsync<UnitOfMeasure>("UnitOfMeasure_Save",
                   param,
                        commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds)).FirstOrDefault();
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("successfully"))
                {
                    response.Code = "0"; // Success
                    response.Data = (UnitOfMeasure) resultData;
                    response.Message = "Save Successfully(BE)";
                }
                else
                {
                    response.Code = "-999"; // Fail
                    response.Message = "Failed(BE)"  +resultMessage;
                }
                return response;
            }
        }
        catch (SqlException sqlex)
        {

            response.Code = "2";
            response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
            return response;
        }
        catch (DbUpdateConcurrencyException ex)
        {

            response.Code = "3";
            response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
            return response;
        }
        catch (DbUpdateException ex)
        {

            response.Code = "4";
            response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
            return response;
        }
        catch (OperationCanceledException ex)
        {

            response.Code = "5";
            response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
            return response;
        }
        catch (Exception ex)
        {

            response.Code = "6";
            response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
            return response;
        }
    }

    public async Task<ResultService<UnitOfMeasure>> GetByCode(string uomCode)
    {
        ResultService<UnitOfMeasure> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<UnitOfMeasure>("UoM_GetByCode",
                    new
                    {
                        UoMCode = uomCode
                    },
                     commandType: CommandType.StoredProcedure,
                     commandTimeout: 240);
                if (rs == null)
                {
                    result.Message = "Failed to get data";
                    result.Code = "1";
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

    public async Task<ResultService<string>> DeleteByDapper(string uomCode)
    {
        ResultService<string> resultService = new();
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {

                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@UoMCode", uomCode);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<VehicleModel>("UoM_Delete",
                   param,
                   commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("OK"))
                {
                    resultService.Code = "0"; // Success
                    resultService.Message = "Deleted Successfully";
                }
                else
                {
                    resultService.Code = "-999";
                    resultService.Message = "Failed";
                    resultService.Message = resultMessage ?? "Failed"; // Sử dụng message từ stored procedure nếu có

                }

                return resultService;
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ResultService<string>()
            {
                Code = "2",
                Data = null,
                Message = $"{ex.GetType()}, {ex.Message}"
            };

        }
        catch (DbUpdateException ex)
        {
            return new ResultService<string>()
            {
                Code = "3",
                Data = null,
                Message = $"{ex.GetType()}, {ex.Message}"
            };
        }
        catch (OperationCanceledException ex)
        {
            return new ResultService<string>()
            {
                Code = "4",
                Data = null,
                Message = $"{ex.GetType()}, {ex.Message}"
            };
        }
        catch (SqlException ex)
        {
            return new ResultService<string>()
            {
                Code = "5",
                Data = null,
                Message = $"{ex.GetType()}, {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new ResultService<string>()
            {
                Code = "6",
                Data = null,
                Message = $"{ex.GetType()}, {ex.Message}"
            };
        }
    }
    #endregion
}




