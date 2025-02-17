﻿using Base.BaseService;
using Base.Example;
using Base.MasterData;
using Base.ProductClassification;
using Base.ProductProperties;
using Context.Example;
using Context.MasterData;
using Context.ProductClassification;
using Context.ProductProperties;
using Core.ExampleClass;
using Core.MasterData;
using Core.ProductClassification;
using Core.ProductProperties;
using Helper.Method;
using Microsoft.EntityFrameworkCore;
using Servicer.Example;
using Servicer.MasterData;
using Servicer.ProductClassification;
using Servicer.ProductProperties;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string chuỗi = General.DecryptString(builder.Configuration.GetConnectionString("DB_RMS")!);
string xâu = General.EncryptString("Data Source=104.197.108.88;Initial Catalog=DB_Inventory;Persist Security Info=True;User ID=sqlserver;Password=codingforever@3003;TrustServerCertificate=True;");
builder.Services.AddDbContext<DB_Testing_Context>(options =>
          options.UseLazyLoadingProxies().UseSqlServer(
                      "Server = 104.197.108.88; Database = Testing; User Id = sqlserver; Password = codingforever@3003; Encrypt = False; TrustServerCertificate = False; MultipleActiveResultSets = true; MultiSubnetFailover = True;",
                      sqlServerOptions => sqlServerOptions.CommandTimeout(360))
        );
builder.Services.AddDbContext<DB_MasterData_Context>(options =>
          options.UseLazyLoadingProxies().UseSqlServer(
                     General.DecryptString(builder.Configuration.GetConnectionString("DB_Inventory")!),
                      sqlServerOptions => sqlServerOptions.CommandTimeout(360))
        );
builder.Services.AddDbContext<DB_ProductProperties_Context>(options =>
          options.UseLazyLoadingProxies().UseSqlServer(
                     General.DecryptString(builder.Configuration.GetConnectionString("DB_Inventory")!),
                      sqlServerOptions => sqlServerOptions.CommandTimeout(360))
        );

builder.Services.AddDbContext<DB_ProductClassification_Context>(options =>
          options.UseLazyLoadingProxies().UseSqlServer(
                     General.DecryptString(builder.Configuration.GetConnectionString("DB_Inventory")!),
                      sqlServerOptions => sqlServerOptions.CommandTimeout(360))
        );


#region Transient Context
builder.Services.AddTransient<DB_Testing_Context>();
builder.Services.AddTransient<DB_MasterData_Context>();
builder.Services.AddTransient<DB_ProductProperties_Context>();
builder.Services.AddTransient<DB_ProductClassification_Context>();
#endregion

#region Add Dependency Injection

#region Example
builder.Services.AddTransient<IMessageContentProvider, MessageContentProvider>();
builder.Services.AddTransient<ICRUD_Service<MessageContent, int>, MessageContentProvider>();
#endregion



#region Master_Data

//Business Partner

builder.Services.AddTransient<IBusinessPartnerProvider, BusinessPartnerProvider>();
builder.Services.AddTransient<ICRUD_Service<BusinessPartner, int>, BusinessPartnerProvider>();
builder.Services.AddTransient<ITransactionTypeProvider, TransactionTypeProvider>();
builder.Services.AddTransient<ICRUD_Service<TransactionType, int>, TransactionTypeProvider>();
#endregion

#region Product_Properties

//Color
builder.Services.AddTransient<IColorProvider, ColorProvider>();
builder.Services.AddTransient<ICRUD_Service<Color, int>, ColorProvider>();
#endregion

#region Product_Classification
//Product Type
builder.Services.AddTransient<IProductTypeProvider, ProductTypeProvider>();
builder.Services.AddTransient<ICRUD_Service<ProductType, int>, ProductTypeProvider>();
//Product Category
builder.Services.AddTransient<IProductCategoryProvider, ProductCategoryProvider>();
builder.Services.AddTransient<ICRUD_Service<ProductCategory, int>, ProductCategoryProvider>();


builder.Services.AddTransient<IStatusMasterProvider, StatusMasterProvider>();
builder.Services.AddTransient<ICRUD_Service<StatusMaster, int>, StatusMasterProvider>();
builder.Services.AddTransient<IWarehouseProvider,WarehouseProvider>();
builder.Services.AddTransient<ICRUD_Service<Warehouse,int>, WarehouseProvider>();
#endregion

#region Warehouse_Management
//
//
#endregion

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
