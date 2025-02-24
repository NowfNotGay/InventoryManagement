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

namespace Servicer.MasterData
{
    public class TransactionTypeProvider : ICRUD_Service<TransactionType, int>, ITransactionTypeProvider
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

        public async Task<TransactionType> Get(int id)
        {
            try
            {
                string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));

                using (var dbconnection = new SqlConnection(connectionString))
                {
                    await dbconnection.OpenAsync();
                    var param = new DynamicParameters();
                    param.Add("@ID", id);
                    return await dbconnection.QueryFirstOrDefaultAsync<TransactionType>("TransactionType_GetByID",
                        param,
                         commandType: CommandType.StoredProcedure,
                            commandTimeout: TimeoutInSeconds);
                }
            }
            catch (SqlException ex)
            {
                return null;
            }


        }

        public async Task<IEnumerable<TransactionType>> GetAll()
        {
            try
            {
                string connectionString = General.DecryptString(_configuration.GetConnectionString(_moduleDapper));

                using (var dbconnection = new SqlConnection(connectionString))
                {
                    await dbconnection.OpenAsync();
                    var param = new DynamicParameters();
                    return await dbconnection.QueryAsync<TransactionType>("TransactionType_GetAll",
                         commandType: CommandType.StoredProcedure,
                            commandTimeout: TimeoutInSeconds);
                }
            }
            catch (SqlException ex)
            {
                return null;
            }


        }

        public async Task<TransactionType> Create(TransactionType entity)
        {

            using (var connection = _Context.Database.BeginTransaction())
            {
                try
                {
                    entity.RowPointer = Guid.NewGuid();
                    await _Context.TransactionTypes.AddAsync(entity);
                    await _Context.SaveChangesAsync();
                    await connection.CommitAsync();
                    return entity;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await connection.RollbackAsync();
                    return null;
                }
                catch (DbUpdateException ex)
                {
                    await connection.RollbackAsync();
                    return null;
                }
                catch (OperationCanceledException ex)
                {
                    await connection.RollbackAsync();
                    return null;
                }

            }

        }



        public async Task<TransactionType> Update(TransactionType entity)
        {
            try
            {
                using (var connection = _Context.Database.BeginTransaction())
                {
                    var TransType = await Get(entity.ID);
                    var newTransType = await _Context.TransactionTypes.FindAsync(TransType.RowPointer);
                    if (newTransType == null)
                        return null;


                    newTransType.Description = entity.Description;
                    newTransType.DocumentTypeID = entity.DocumentTypeID;
                    newTransType.TransactionTypeCode = entity.TransactionTypeCode;
                    newTransType.TransactionTypeName = entity.TransactionTypeName;
                    newTransType.UpdatedBy = entity.UpdatedBy;
                    newTransType.UpdatedDate = DateTime.Now;



                    await _Context.SaveChangesAsync();
                    await connection.CommitAsync();
                    return newTransType;



                }
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
            catch (ArgumentException ex)
            {
                return null;
            }
            catch (OperationCanceledException ex)
            {
                return null;
            }

        }
        public async Task<string> Delete(int id)
        {
            try
            {
                var entity = await this.Get(id);
                if (entity == null)
                {
                    throw new Exception("Entity not found!");
                }

                _Context.TransactionTypes.Remove(entity);
                await _Context.SaveChangesAsync();
                return "Entity deleted successfully";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return $"{ex.GetType()}, {ex.Message}";
            }
            catch (DbUpdateException ex)
            {
                return $"{ex.GetType()}, {ex.Message}";
            }
            catch (OperationCanceledException ex)
            {
                return $"{ex.GetType()}, {ex.Message}";
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
