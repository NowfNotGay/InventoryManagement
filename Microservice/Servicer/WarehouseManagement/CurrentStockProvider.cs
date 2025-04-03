using Base.BaseService;
using Base.WarehouseManagement;
using Context.WarehouseManagement;
using Core.BaseClass;
using Core.WarehouseManagement;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace Servicer.WarehouseManagement
{
	public class CurrentStockProvider : ICRUD_Service<CurrentStock, int>, ICurrentStockProvider
	{
		private readonly DB_WarehouseManagement_Context _Context;
		private readonly IConfiguration _configuration;
		private string _moduleDapper = "DB_Inventory_DAPPER";
		private const int TimeoutInSeconds = 240;
        private string currentStockType = "UDTT_CurrentStock";



        public CurrentStockProvider(DB_WarehouseManagement_Context context, IConfiguration configuration)
		{
			_Context = context;
			_configuration = configuration;
		}
        #region ICRUD
        public async Task<ResultService<CurrentStock>> Create(CurrentStock entity)
		{
            var response = new ResultService<CurrentStock>();
            if (entity == null)
            {
                return new ResultService<CurrentStock>()
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
                    await _Context.CurrentStocks.AddAsync(entity);
                    await _Context.SaveChangesAsync();
                    await connection.CommitAsync();

                    response.Code = "0";
                    response.Message = "Create new Instance Successfully";
                    response.Data = entity;
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

        public async Task<ResultService<string>> Delete(int id)
        {
            var param = new DynamicParameters();
            param.Add("@ID", id);
            var result = await this.CallStoreSingle<String>("Delete_CurrentStock", param);
            return result;
        }
        public async Task<ResultService<CurrentStock>> Get(int id)
        {
            var param = new DynamicParameters();
            param.Add("@ID", id);
            var result = await this.CallStoreSingle<CurrentStock>("CurrentStock_GetByID",param);
            if(result.Data == null)
            {
                return new ResultService<CurrentStock>()
                {
                    Code = "-1",
                    Message = "Entity not found",
                    Data = null
                };
            }
            return result;
        }
        public async Task<ResultService<IEnumerable<CurrentStock>>> GetAll()
		{
            return await this.CallStoreArray<CurrentStock>("CurrentStock_GetAll");
		}

		public async Task<ResultService<CurrentStock>> Update(CurrentStock entity)
		{
            var response = new ResultService<CurrentStock>();

            var getEntityID = await this.Get(entity.ID);
            if (getEntityID.Code == "-1")
            {
                return new ResultService<CurrentStock>()
                {
                    Code = getEntityID.Code,
                    Message = "Entity not found"
                };

            }
            var newEntity = await _Context.CurrentStocks.FindAsync(getEntityID.Data.RowPointer);
            if (newEntity == null)
            {
                return new ResultService<CurrentStock>()
                {
                    Code = "-1",
                    Message = "Entity not found"
                };

            }
            using (var connection = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    newEntity.WarehouseID = entity.WarehouseID;
                    newEntity.UpdatedDate = DateTime.UtcNow;
                    newEntity.UpdatedBy = entity.UpdatedBy;
                    newEntity.ProductVariantID = entity.ProductVariantID;
                    newEntity.UoMID = entity.UoMID;
                    newEntity.Quantity = entity.Quantity;
                    newEntity.StorageBinID = entity.StorageBinID;
                    newEntity.CreatedBy = entity.CreatedBy;
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


        #endregion
        private async Task<ResultService<IEnumerable<T>>> CallStoreArray<T>(String nameStore, DynamicParameters? param = null )
        {
            var response = new ResultService<IEnumerable<T>>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                     var result = param != null 
                        ? await sqlConnection.QueryAsync<T>(nameStore, param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds) 
                        : await sqlConnection.QueryAsync<T>(nameStore, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
                    response.Code = "0";
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
            catch (OperationCanceledException ex)
            {
                //Lỗi xuất hiện do timeout hoặc yêu cầu dừng quá trình.
                response.Code = "3";
                response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "999";
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        private async Task<ResultService<T>> CallStoreSingle<T>(String nameStore, DynamicParameters? param = null)
        {
            var response = new ResultService<T>();
            string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    var result = param != null 
                        ? await sqlConnection.QuerySingleOrDefaultAsync<T>(nameStore,
                            param,
                            commandType: CommandType.StoredProcedure,
                               commandTimeout: TimeoutInSeconds) 
                        : await sqlConnection.QuerySingleOrDefaultAsync<T>(nameStore,
                            commandType: CommandType.StoredProcedure,
                               commandTimeout: TimeoutInSeconds);
                    response.Code = "0";
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
                //Lỗi xuất hiện do timeout hoặc yêu cầu dừng quá trình.
                response.Code = "5";
                response.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "999";
                response.Message = $"An error occurred while executing store Procedure. Details: {ex.GetType()} - {ex.Message}";
                return response;
            }
        }

        public async Task<ResultService<CurrentStock>> Save(UDTT_CurrentStock entity)
        {
            var param = new DynamicParameters();
            

            // Thêm parameter @SaveBy
            param.Add("@SaveBy", "Thangh", DbType.String);

            // Thêm parameter dạng table-valued (@currentStock)
            var datatableList = new List<UDTT_CurrentStock>();
            datatableList.Add(entity);
            var dataTable = General.ConvertToDataTable(datatableList); // Chuyển entity thành DataTable
            param.Add("@currentStock", dataTable.AsTableValuedParameter(this.currentStockType));

            // Thêm output parameters
            param.Add("@Status", dbType: DbType.Byte, direction: ParameterDirection.Output);
            param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            var result = await CallStoreSingle<CurrentStock>("Save_CurrentStock", param);
          
            return result;
        }
    }
}
