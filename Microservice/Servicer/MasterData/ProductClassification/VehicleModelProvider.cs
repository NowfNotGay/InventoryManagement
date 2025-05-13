using Base.BaseService;
using Base.MasterData.ProductClassification;
using Context.MasterData.ProductClassification;
using Context.ProductProperties;
using Core.BaseClass;
using Core.MasterData.ProductClassification;
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

namespace Servicer.MasterData.ProductClassification;
public class VehicleModelProvider : ICRUD_Service<VehicleModel, int>, IVehicleModelProvider
{
    private readonly DB_ProductClassification_Context _dB;
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString;
    private const int TimeoutInSeconds = 240;

    public VehicleModelProvider(DB_ProductClassification_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER")!);
    }
    #region Base CRUD

    public async Task<ResultService<VehicleModel>> Create(VehicleModel entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            ResultService<VehicleModel> result = new();
            try
            {
                await _dB.VehicleModels.AddAsync(entity);


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
                result.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
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

    public async Task<ResultService<VehicleModel>> Update(VehicleModel entity)
    {
        ResultService<VehicleModel> result = new();

        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.VehicleModels.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }
                // Update properties as necessary
                obj.ModelCode = entity.ModelCode;
                obj.ModelName = entity.ModelName;
                obj.BrandCode = entity.BrandCode;
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
                result.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
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
                _dB.VehicleModels.Remove(obj.Data);
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
                result.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
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

    #endregion



    #region Dapper CRUD
    public async Task<ResultService<VehicleModel>> Get(int id)
    {
        ResultService<VehicleModel> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<VehicleModel>("VehicleModel_GetByID",
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
    }

    public async Task<ResultService<IEnumerable<VehicleModel>>> GetAll()
    {
        ResultService<IEnumerable<VehicleModel>> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<VehicleModel>("VehicleModel_GetAll",
                    new
                    {

                    },
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
    }


    public async Task<ResultService<VehicleModel>> SaveByDapper(VehicleModel entity)
    {
        var response = new ResultService<VehicleModel>();

        if (entity == null)
        {
            return new ResultService<VehicleModel>()
            {
                Code = "1",
                Message = "Entity is not valid(BE)"
            };
        }
        try
        {
            string Message = string.Empty;
            entity.ModelCode = !entity.ModelCode.Contains("MOD") ? string.Empty : entity.ModelCode;
            List<VehicleModel> lst = new List<VehicleModel>();
            entity.RowPointer = Guid.Empty;
            lst.Add(entity);

            DataTable dtHeader = General.ConvertToDataTable(lst);
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();

                param.Add("@CreatedBy", entity.CreatedBy);
                param.Add("@udtt_VehicleModel", dtHeader.AsTableValuedParameter("UDTT_VehicleModel"));

                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<VehicleModel>("VehicleModel_Save",
                   param,
                   commandType: CommandType.StoredProcedure,
                      commandTimeout: TimeoutInSeconds);
                var resultMessage = param.Get<string>("@Message");

                if (resultMessage.Contains("successfully"))
                {
                    response.Code = "0"; // Success
                    response.Message = "Save Successfully(BE)";
                }
                else
                {
                    response.Code = "-999"; // Fail
                    response.Message = "Failed(BE)";
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

    public async Task<ResultService<VehicleModel>> GetByCode(string modelCode)
    {
        ResultService<VehicleModel> result = new();
        using (var sqlconnect = new SqlConnection(_dapperConnectionString))
        {
            try
            {
                await sqlconnect.OpenAsync();
                var rs = await sqlconnect.QuerySingleOrDefaultAsync<VehicleModel>("VehicleModel_GetByCode",
                    new
                    {
                        ModelCode = modelCode
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

    public async Task<ResultService<string>> DeleteByDapper(string modelCode)
    {
        ResultService<string> resultService = new();
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {

                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@ModelCode", modelCode);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<VehicleModel>("VehicleModel_Delete",
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


