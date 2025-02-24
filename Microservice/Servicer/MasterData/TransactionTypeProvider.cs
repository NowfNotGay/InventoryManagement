using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BaseService;
using Base.MasterData;
using Context.MasterData;
using Microsoft.Extensions.Configuration;
using Core.MasterData;
using Dapper;
using Microsoft.Data.SqlClient;
using Helper.Method;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Core.BaseClass;
using System.Data.Common;

namespace Servicer.MasterData
{
    public class TransactionTypeProvider : ICRUD_Service_V2<TransactionType, int>, ITransactionTypeProvider
    {
        private readonly DB_MasterData_Context _Context;
        private readonly IConfiguration _configuration;
        private string _moduleDapper = "DB_Inventory_DAPPER";
        private const int TimeoutInSeconds = 240;

        public TransactionTypeProvider(DB_MasterData_Context context, IConfiguration configuration)
        {
            _Context = context;
            _configuration = configuration;

        }

        public async Task<ResultService<IEnumerable<TransactionType>>> GetAll()
        {
            var response = new ResultService<IEnumerable<TransactionType>>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var result = await sqlConnection.QueryAsync<TransactionType>("TransactionType_GetAll",
                        commandType: CommandType.StoredProcedure,
                           commandTimeout: TimeoutInSeconds);

                    response.Code = "0";// Success
                    response.Message = "Success";
                    response.Data = result;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.Code = "1";
                response.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Code = "2";
                response.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "999";
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<TransactionType>> Get(int id)
        {
            var response = new ResultService<TransactionType>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@ID", id);
                    var result = await sqlConnection.QuerySingleOrDefaultAsync<TransactionType>("TransactionType_GetByID",
                        param,
                        commandType: CommandType.StoredProcedure,
                           commandTimeout: TimeoutInSeconds);
                    if(result == null)
                    {
                        return new ResultService<TransactionType>()
                        {
                            Code = "-1",
                            Message = "Entity not found",
                            Data = null
                        };
                    }
                    response.Code = "0";// Success
                    response.Message = "Success";
                    response.Data = result;
                    return response;
                }
            }
            catch (SqlException sqlEx)
            {
                response.Code = "1";
                response.Message = $"{sqlEx.GetType()} - {sqlEx.Message}";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Code = "2";
                response.Message = $"An error occurred while trying to connect to your database Server, pls check your Configuration .Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "999";
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<TransactionType>> Create(TransactionType entity)
        {
            var response = new ResultService<TransactionType>();
            if (entity == null)
            {
                return new ResultService<TransactionType>()
                {
                    Code = "1",
                    Message = "Entity is not valid"
                };
            }

            using (var connection = _Context.Database.BeginTransaction())
            {
                try
                {
                    entity.RowPointer = Guid.Empty;
                    entity.ID = 0;
                    await _Context.TransactionTypes.AddAsync(entity);
                    await _Context.SaveChangesAsync();
                    await connection.CommitAsync();

                    response.Code = "0";// Success
                    response.Message = "Create new Instance Successfully";
                    response.Data = entity;
                    return response;
                }
                catch (SqlException sqlex)
                {
                    await connection.RollbackAsync();
                    //lỗi xảy ra khi có sự xung đột giữa các bản ghi trong cơ sở dữ liệu khi cố gắng cập nhật.
                    response.Code = "2";
                    response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                    return response;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await connection.RollbackAsync();
                    //lỗi xảy ra khi có sự xung đột giữa các bản ghi trong cơ sở dữ liệu khi cố gắng cập nhật.
                    response.Code = "3";
                    response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (DbUpdateException ex)
                {
                    await connection.RollbackAsync();
                    //lỗi xảy ra khi không thể cập nhật cơ sở dữ liệu, có thể do các vấn đề về dữ liệu hoặc các ràng buộc.
                    response.Code = "4";
                    response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (OperationCanceledException ex)
                {
                    await connection.RollbackAsync();
                    //Lỗi xuất hiện do timeout hoặc yêu cầu dừng quá trình.
                    response.Code = "5";
                    response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (Exception ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "6";
                    response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                    return response;
                }
            }

        }

        public async Task<ResultService<TransactionType>> Update(TransactionType entity)
        {
            var response = new ResultService<TransactionType>();

            var getEntityID = await this.Get(entity.ID);
            if (getEntityID.Code == "-1")
            {
                return new ResultService<TransactionType>()
                {
                    Code = getEntityID.Code,
                    Message = "Entity not found"
                };

            }
            var newEntity = await _Context.TransactionTypes.FindAsync(getEntityID.Data.RowPointer);
            if (newEntity == null)
            {
                return new ResultService<TransactionType>()
                {
                    Code = "-1",
                    Message = "Entity not found"
                };

            }
            using (var connection = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    newEntity.Description = entity.Description;
                    newEntity.UpdatedDate = DateTime.UtcNow;
                    newEntity.UpdatedBy = entity.UpdatedBy;
                    newEntity.DocumentTypeID = entity.DocumentTypeID;
                    newEntity.TransactionTypeCode = entity.TransactionTypeCode;
                    newEntity.TransactionTypeName = entity.TransactionTypeName;

                    await _Context.SaveChangesAsync();
                    await connection.CommitAsync();
                    response.Code = "0";
                    response.Data = newEntity;
                    response.Message = "Update entity Successfully";
                    return response;
                }
                catch (SqlException sqlex)
                {
                    await connection.RollbackAsync();
                    response.Code = "2";
                    response.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
                    return response;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "3";
                    response.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (DbUpdateException ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "4";
                    response.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (OperationCanceledException ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "5";
                    response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                    return response;
                }
                catch (Exception ex)
                {
                    await connection.RollbackAsync();
                    response.Code = "6";
                    response.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
                    return response;
                }

            }
        }


        async Task<ResultService<string>> ICRUD_Service_V2<TransactionType, int>.Delete(int id)
        {
            ResultService<string> resultService = new ResultService<string>();
            try
            {
                var entity = await this.Get(id);
                if (entity.Code == "-1")
                {
                    return new ResultService<string>()
                    {
                        Code = entity.Code,
                        Data = null,
                        Message = "Entity not found"
                    };
                }

                _Context.TransactionTypes.Remove(entity.Data);
                await _Context.SaveChangesAsync();
                resultService.Code = "0";
                resultService.Message = "Entity deleted successfully";
                return resultService;
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

        Task<ResultService<TransactionType>> ICRUD_Service<TransactionType, int>.Create(TransactionType entity)
        {
            throw new NotImplementedException();
        }

        Task<ResultService<TransactionType>> ICRUD_Service<TransactionType, int>.Update(TransactionType entity)
        {
            throw new NotImplementedException();
        }

        Task<ResultService<string>> ICRUD_Service<TransactionType, int>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResultService<TransactionType>> ICRUD_Service<TransactionType, int>.Get(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResultService<IEnumerable<TransactionType>>> ICRUD_Service<TransactionType, int>.GetAll()
        {
            throw new NotImplementedException();
        }


    }
}
