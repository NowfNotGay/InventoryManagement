using Base.BaseService;
using Base.Example;
using Context.Example;
using Core.ExampleClass;
using Helper.Method;
using Microsoft.EntityFrameworkCore;
using Servicer.Example;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string chuỗi = General.DecryptString(builder.Configuration.GetConnectionString("DB_RMS")!);
builder.Services.AddDbContext<DB_Testing_Context>(options =>
          options.UseLazyLoadingProxies().UseSqlServer(
                      "Server = 104.197.108.88; Database = Testing; User Id = sqlserver; Password = codingforever@3003; Encrypt = False; TrustServerCertificate = False; MultipleActiveResultSets = true; MultiSubnetFailover = True;",
                      sqlServerOptions => sqlServerOptions.CommandTimeout(360))
        );

#region Transient Context
builder.Services.AddTransient<DB_Testing_Context>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Add Dependency Injection
#region Example
builder.Services.AddTransient<IMessageContentProvider, MessageContentProvider>();
builder.Services.AddTransient<ICRUD_Service<MessageContent, int>, MessageContentProvider>();
#endregion


#endregion


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
