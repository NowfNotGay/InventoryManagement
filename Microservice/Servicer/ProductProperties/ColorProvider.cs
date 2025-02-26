using Base.BaseService;
using Base.ProductProperties;
using Context.ProductProperties;
using Core.BaseClass;
using Core.MasterData;
using Core.ProductProperties;
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
using System.Transactions;

namespace Servicer.ProductProperties;
public class ColorProvider : ICRUD_Service<Color, int>, IColorProvider
{
    private readonly DB_ProductProperties_Context _dB;
    private readonly IConfiguration _configuration;

    public ColorProvider(DB_ProductProperties_Context dB, IConfiguration configuration)
    {
        _dB = dB;
        _configuration = configuration;
    }

    public async Task<ResultService<Color>> Create(Color entity)
    {
        using (var transaction = _dB.Database.BeginTransaction())
        {
            ResultService<Color> result = new();
            try
            {
                await _dB.Colors.AddAsync(entity);
                
                 
                if(_dB.SaveChanges() <= 0)
                {
                    result.Message = "Failed to create data";
                    result.Code = "1";
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = entity;
                return result;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                switch (ex.Number)
                {
                    case 53:
                        result.Code = "1001";
                        result.Message = "Database connection failed";
                        break;

                    case 208:  
                        result.Code = "1007";
                        result.Message = "SQL Error: Table or column not found";
                        break;

                    case 156: 
                        result.Code = "1005";
                        result.Message = "SQL syntax error";
                        break;

                    case 1205:
                        result.Code = "1006";
                        result.Message = "Deadlock occurred, transaction rolled back";
                        break;

                    default:
                        result.Code = "1099";
                        result.Message = $"SQL Error: {ex.Message}";
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "1";
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
                var obj = await Get(id, CommandType);
                if (!obj.Code.Equals("0"))
                {
                    result.Message = obj.Message;
                    result.Code = obj.Code;
                    result.Data = "false";
                    return result;
                    
                }
                _dB.Colors.Remove(obj.Data);
                if (_dB.SaveChanges() <=0)             
                {
                    result.Message = "Failed to delete data";
                    result.Code = "1";
                    result.Data = "false";
                    return result;
                }
                await transaction.CommitAsync();
                result.Message = "Success";
                result.Code = "0";
                result.Data = "true";
                return result;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                switch (ex.Number)
                {
                    case 53:
                        result.Code = "1001";
                        result.Message = "Database connection failed";
                        break;

                    case 208:
                        result.Code = "1007";
                        result.Message = "SQL Error: Table or column not found";
                        break;

                    case 156:
                        result.Code = "1005";
                        result.Message = "SQL syntax error";
                        break;

                    case 1205:
                        result.Code = "1006";
                        result.Message = "Deadlock occurred, transaction rolled back";
                        break;

                    default:
                        result.Code = "1099";
                        result.Message = $"SQL Error: {ex.Message}";
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "1";
                return result;
            }
            
        }
    }

    public async Task<ResultService<Color>> Get(int id)
    {
        ResultService<Color> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            result.Data = await sqlconnect.QuerySingleOrDefaultAsync<Color>("Color_GetByID",
                new
                {
                    ID = id
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
    }

    public async Task<ResultService<IEnumerable<Color>>> GetAll()
    {
        ResultService<IEnumerable<Color>> result = new();
        using (var sqlconnect = new SqlConnection(General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"))))
        {
            await sqlconnect.OpenAsync();
            result.Data = await sqlconnect.QueryAsync<Color>("Color_GetAll",
                new
                {

                },
                 commandType: CommandType.StoredProcedure,
                 commandTimeout: 240);
            if(result.Data == null)
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
    }

    public async Task<ResultService<Color>> Update(Color entity)
    {
        ResultService<Color> result = new();
        using (var transaction = _dB.Database.BeginTransaction())
        {
            try
            {
                var obj = await _dB.Colors.FindAsync(entity.RowPointer);
                if (obj == null)
                {
                    result.Message = "Data not found!";
                    result.Code = "1";
                    result.Data = null;
                    return result;
                }
                obj.ColorCode = entity.ColorCode;
                obj.ColorName = entity.ColorName;
                obj.UpdatedBy = entity.UpdatedBy;
                obj.UpdatedDate = DateTime.Now;

                
                if( _dB.SaveChanges() <=0)
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
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                switch (ex.Number)
                {
                    case 53:
                        result.Code = "1001";
                        result.Message = "Database connection failed";
                        break;

                    case 208:
                        result.Code = "1007";
                        result.Message = "SQL Error: Table or column not found";
                        break;

                    case 156:
                        result.Code = "1005";
                        result.Message = "SQL syntax error";
                        break;

                    case 1205:
                        result.Code = "1006";
                        result.Message = "Deadlock occurred, transaction rolled back";
                        break;

                    default:
                        result.Code = "1099";
                        result.Message = $"SQL Error: {ex.Message}";
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Message = ex.Message;
                result.Code = "1";
                return result;
 
            }
        }
    }
}
