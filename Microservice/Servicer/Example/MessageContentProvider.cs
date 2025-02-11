using Base.BaseService;
using Base.Example;
using Context.Example;
using Core.ExampleClass;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Servicer.Example
{
    public class MessageContentProvider : IMessageContentProvider, 
                                            ICRUD_Service<MessageContent, int>
    {
        private readonly DB_Testing_Context _databaseContext;

        public MessageContentProvider(DB_Testing_Context databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<MessageContent> Create(MessageContent entity)
        {
            using (var transaction = _databaseContext.Database.BeginTransaction())
            {
                try
                {
                    await _databaseContext.MessageContents.AddAsync(entity);
                    int change = await _databaseContext.SaveChangesAsync();
                        
                    if(change <= 0)
                    {
                        throw new Exception("Failed to save data");
                    }
                    await transaction.CommitAsync();
                    return entity;
                }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();
                    return null;
                }
            }
        }

        public Task<MessageContent> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageContent> Get(int id)
        {
            using (var sqlConnection = new SqlConnection("Data Source=104.197.108.88;Initial Catalog=Testing;Persist Security Info=True;User ID=sqlserver;Password=codingforever@3003;TrustServerCertificate=True;"))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    //var parameters = new DynamicParameters();


                    var result = await sqlConnection.QuerySingleOrDefaultAsync<MessageContent>("MessageContents_GetByID",
                        new
                        {
                            ID = id
                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 240);


                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<IEnumerable<MessageContent>> GetAll()
        {
            using (var sqlConnection = new SqlConnection("Data Source=104.197.108.88;Initial Catalog=Testing;Persist Security Info=True;User ID=sqlserver;Password=codingforever@3003;TrustServerCertificate=True;"))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    //var parameters = new DynamicParameters();
                  

                    var result = await sqlConnection.QueryAsync<MessageContent>("MessageContents_GetAll",
                        new
                        {

                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 240);

                    
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public Task<IEnumerable<MessageContent>> GetByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public Task<MessageContent> Update(MessageContent entity)
        {
            throw new NotImplementedException();
        }

        Task<string> ICRUD_Service<MessageContent, int>.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
