using Base.BaseService;
using Base.Example;
using Base.MasterData;
using Base.ProductClassification;
using Base.ProductManagement;
using Base.ProductProperties;
using Base.WarehouseManagement;
using Context.Example;
using Context.MasterData;
using Context.ProductClassification;
using Context.ProductManagement;
using Context.ProductProperties;
using Context.WarehouseManagement;
using Core.ExampleClass;
using Core.MasterData;
using Core.ProductClassification;
using Core.ProductManagement;
using Core.ProductProperties;
using Core.WarehouseManagement;
using Helper.Method;
using Microsoft.EntityFrameworkCore;
using Servicer.Example;
using Servicer.MasterData;
using Servicer.ProductClassification;
using Servicer.ProductManagement;
using Servicer.ProductProperties;
using Servicer.WarehouseManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string chuỗi = General.DecryptString(builder.Configuration.GetConnectionString("DB_Inventory")!);
string xâu = General.EncryptString(builder.Configuration.GetConnectionString("DB_Inventory_DAPPER")!);
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
builder.Services.AddDbContext<DB_ProductManagement_Context>(options =>
          options.UseLazyLoadingProxies().UseSqlServer(
                     General.DecryptString(builder.Configuration.GetConnectionString("DB_Inventory")!),
                      sqlServerOptions => sqlServerOptions.CommandTimeout(360))
        );
builder.Services.AddDbContext<DB_WarehouseManagement_Context>(options =>
          options.UseLazyLoadingProxies().UseSqlServer(
                     General.DecryptString(builder.Configuration.GetConnectionString("DB_Inventory")!),
                      sqlServerOptions => sqlServerOptions.CommandTimeout(360))
        );


#region Transient Context
builder.Services.AddTransient<DB_Testing_Context>();
builder.Services.AddTransient<DB_MasterData_Context>();
builder.Services.AddTransient<DB_ProductProperties_Context>();
builder.Services.AddTransient<DB_ProductClassification_Context>();
builder.Services.AddTransient<DB_WarehouseManagement_Context>();
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
//Transaction Type
builder.Services.AddTransient<ITransactionTypeProvider, TransactionTypeProvider>();
builder.Services.AddTransient<ICRUD_Service<TransactionType, int>, TransactionTypeProvider>();
//Status Master - Hai
builder.Services.AddTransient<IStatusMasterProvider, StatusMasterProvider>();
builder.Services.AddTransient<ICRUD_Service<StatusMaster, int>, StatusMasterProvider>();
//Warehouse - Hai
builder.Services.AddTransient<IWarehouseProvider, WarehouseProvider>();
builder.Services.AddTransient<ICRUD_Service<Warehouse, int>, WarehouseProvider>();
//StorageBin - Duy
builder.Services.AddTransient<IStorageBinProvider, StorageBinProvider>();
builder.Services.AddTransient<ICRUD_Service<StorageBin, int>, StorageBinProvider>();

#endregion

#region Product_Properties

//Color
builder.Services.AddTransient<IColorProvider, ColorProvider>();
builder.Services.AddTransient<ICRUD_Service<Color, int>, ColorProvider>();
//Material - Bao
builder.Services.AddTransient<IMaterialProvider, MaterialProvider>();
builder.Services.AddTransient<ICRUD_Service<Material, int>, MaterialProvider>();

//Dimension - Bao
builder.Services.AddTransient<IDimensionProvider, DimensionProvider>();
builder.Services.AddTransient<ICRUD_Service<Dimension, int>, DimensionProvider>();

//Unit Of Measure - Hai
builder.Services.AddTransient<IUnitOfMeasureProvider, UnitOfMeasureProvider>();
builder.Services.AddTransient<ICRUD_Service<UnitOfMeasure, int>, UnitOfMeasureProvider>();
#endregion

#region Product_Classification
//Product Type - Bao
builder.Services.AddTransient<IProductTypeProvider, ProductTypeProvider>();
builder.Services.AddTransient<ICRUD_Service<ProductType, int>, ProductTypeProvider>();
//Product Category
builder.Services.AddTransient<IProductCategoryProvider, ProductCategoryProvider>();
builder.Services.AddTransient<ICRUD_Service<ProductCategory, int>, ProductCategoryProvider>();
//Brand - Hai
builder.Services.AddTransient<IBrandProvider, BrandProvider>();
builder.Services.AddTransient<ICRUD_Service<Brand, int>, BrandProvider>();
//Vehicle Model - Hai
builder.Services.AddTransient<IVehicleModelProvider, VehicleModelProvider>();
builder.Services.AddTransient<ICRUD_Service<VehicleModel, int>, VehicleModelProvider>();
#endregion

#region Product_Management
//Product - Bao
builder.Services.AddTransient<IProductProvider, ProductProvider>();
builder.Services.AddTransient<ICRUD_Service<Product, int>, ProductProvider>();
//Product Variant - Duy
builder.Services.AddTransient<IProductVariantProvider, ProductVariantProvider>();
builder.Services.AddTransient<ICRUD_Service<ProductVariant, int>, ProductVariantProvider>();
//Product Attribute - Duy
builder.Services.AddTransient<IProductAttributeProvider, ProductAttributeProvider>();
builder.Services.AddTransient<ICRUD_Service<ProductAttribute, int>, ProductAttributeProvider>();

//Product UoM Conversion- Hai
builder.Services.AddTransient<IProductUoMConversionProvider, ProductUoMConversionProvider>();
builder.Services.AddTransient<ICRUD_Service<ProductUoMConversion, int>, ProductUoMConversionProvider>();
#endregion

#region Warehouse_Management
//
builder.Services.AddTransient<ICRUD_Service<GoodsReceiptNote, int>, GoodsReceiptNoteProvider>();
builder.Services.AddTransient<IGoodsReceiptNoteProvider, GoodsReceiptNoteProvider>();
builder.Services.AddTransient<ICRUD_Service<CurrentStock, int>, CurrentStockProvider>();
builder.Services.AddTransient<ICurrentStockProvider, CurrentStockProvider>();

//
//Good Issue Note - Hai
builder.Services.AddTransient<ICRUD_Service<GoodsIssueNote, int>, GoodsIssueNoteProvider>();
builder.Services.AddTransient<IGoodsIssueNoteProvider, GoodsIssueNoteProvider>();
#endregion
//Stock Transfer - Duy
builder.Services.AddTransient<ICRUD_Service<StockTransfer, int>, StockTransferProvider>();
builder.Services.AddTransient<IStockTransferProvider, StockTransferProvider>();


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
