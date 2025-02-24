using Base.BaseService;
using Base.Example;
using Context.Example;
using Core.BaseClass;
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

        public async Task<ResultService<MessageContent>> Get(int id)
        {
            ResultService<MessageContent> result = new();
            using (var sqlConnection = new SqlConnection("Data Source=172.16.10.18,14332;Initial Catalog=Testing;Persist Security Info=True;User ID=sql_Trainning;Password=Dpt@3003;TrustServerCertificate=True;"))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    //var parameters = new DynamicParameters();


                    result.Data = await sqlConnection.QuerySingleOrDefaultAsync<MessageContent>("MessageContents_GetByID",
                        new
                        {
                            ID = id
                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 240);
                    if(result.Data != null)
                    {
                        result.Message = "Success";
                        result.Code = "0";
                    }
                    else
                    {
                        result.Message = "Failed to get data";
                        result.Code = "1";
                    }


                    return result;
                }
                catch (SqlException sqlex)
                {
                    result.Message = sqlex.Message;
                    result.Code = "1";
                    result.Data = null;
                    return result;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.Code = "1";
                    result.Data = null;
                    return result;
                }
            }
        }

        public async Task<ResultService<IEnumerable<MessageContent>>> GetAll()
        {

            ResultService <IEnumerable<MessageContent>> result = new();
            using (var sqlConnection = new SqlConnection("Data Source=104.197.108.88;Initial Catalog=Testing;Persist Security Info=True;User ID=sqlserver;Password=codingforever@3003;TrustServerCertificate=True;"))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    //var parameters = new DynamicParameters();
                  

                    result.Data = await sqlConnection.QueryAsync<MessageContent>("MessageContents_GetAll",
                        new
                        {

                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 240);

                    if (result.Data != null)
                    {
                        result.Message = "Success";
                        result.Code = "0";
                    }
                    else
                    {
                        result.Message = "Failed to get data";
                        result.Code = "1";
                    }
                        return result;
                }
                catch (SqlException sqlex)
                {
                    result.Message = sqlex.Message;
                    result.Code = "1";
                    result.Data = null;
                    return result;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.Code = "1";
                    result.Data = null;
                    return result;
                }
            }
        }

        public Task<IEnumerable<MessageContent>> GetByTitle(string title)
        {
            throw new NotImplementedException();
        }

        

        Task<ResultService<MessageContent>> ICRUD_Service<MessageContent, int>.Create(MessageContent entity)
        {
            ResultService<MessageContent> resultService = new();
            using (var transaction = _databaseContext.Database.BeginTransaction())
            {
                try
                {
                    _databaseContext.MessageContents.Add(entity);
                    int change = _databaseContext.SaveChanges();
                    if (change <= 0)
                    {
                        resultService.Message = "Failed to save data";
                        resultService.Code = "1";
                        resultService.Data = null;
                        return Task.FromResult(resultService);
                    }
                    transaction.Commit();
                    resultService.Message = "Success";
                    resultService.Code = "0";
                    resultService.Data = entity;
                    return Task.FromResult(resultService);
                }
                catch (SqlException sqlex)
                {
                    return Task.FromResult(resultService);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    resultService.Message = ex.Message;
                    resultService.Code = "1";
                    resultService.Data = null;
                    return Task.FromResult(resultService);
                }
            }

        }

       

        public async Task<ResultService<string>> Delete(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<ResultService<MessageContent>> Update(MessageContent entity)
        {
            throw new NotImplementedException();
        }
    }
}
