﻿using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Core.BaseClass;
using Core.MasterData;
using Dapper;
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

    public async Task<ResultService<StorageBin>> Create(StorageBin entity)
    {
        using (var transaction = _db.Database.BeginTransaction())
        {
            ResultService<StorageBin> result = new();
            try
            {
                await _db.StorageBins.AddAsync(entity);
                if (_db.SaveChanges() <= 0)
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
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var entity = await Get(id);
                if (!entity.Code.Equals("0"))
                {
                    result.Message = entity.Message;
                    result.Code = entity.Code;
                    result.Data = "false";
                    return result;
                }

                _db.StorageBins.Remove(entity.Data);
                if (_db.SaveChanges() <= 0)
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
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "999";
                return result;
            }
        }
    }

    public async Task<ResultService<StorageBin>> Get(int id)
    {
        ResultService<StorageBin> result = new();
        using (var sqlConnection = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlConnection.OpenAsync();
                var rs = await sqlConnection.QuerySingleOrDefaultAsync<StorageBin>("StorageBin_GetByID",
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
                    result.Data = rs;
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

    public async Task<ResultService<IEnumerable<StorageBin>>> GetAll()
    {
        ResultService<IEnumerable<StorageBin>> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            try
            {
                await sqlconnect.OpenAsync();
                result.Data = await sqlconnect.QueryAsync<StorageBin>("StorageBin_GetAll",
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

    public async Task<ResultService<StorageBin>> Update(StorageBin entity)
    {
        ResultService<StorageBin> result = new();
        using (var transaction = _db.Database.BeginTransaction())
        {
            try
            {
                var newObj = await _db.StorageBins.FindAsync(entity.RowPointer);
                if (newObj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "-1";
                    result.Data = null;
                    return result;
                }
                newObj.StorageBinCode = entity.StorageBinCode;
                newObj.Description = entity.Description;
                newObj.WarehouseID = entity.WarehouseID;
                newObj.UpdatedDate = DateTime.Now;
                newObj.UpdatedBy = entity.UpdatedBy;



                if (_db.SaveChanges() <= 0)
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
