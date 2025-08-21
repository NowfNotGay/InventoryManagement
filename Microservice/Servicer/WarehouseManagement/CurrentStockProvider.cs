using Base.WarehouseManagement;
using Core.BaseClass;
using Core.MasterData.ProductClassification;
using Core.MasterData.ProductProperties;
using Core.MasterData;
using Core.ProductManagement;
using Core.WarehouseManagement;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Context.MasterData.ProductClassification;
using Helper.Method;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Servicer.WarehouseManagement;
public class CurrentStockProvider : ICurrentStockProvider
{
    private readonly IConfiguration _configuration;
    private readonly string _dapperConnectionString;
    private const int TimeoutInSeconds = 240;

    public CurrentStockProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"));
    }
    public async Task<ResultService<IEnumerable<CurrentStockParam>>> GetAllCurrentStockByWarehouse(string warehouseCode)
    {
        var result = new ResultService<IEnumerable<CurrentStockParam>>();

        try
        {
            using (var conn = new SqlConnection(_dapperConnectionString))
            {
                await conn.OpenAsync();

                var stockList = (await conn.QueryAsync<
                    CurrentStockParam,
                    ProductParam,
                    UnitOfMeasure,
                    Warehouse,
                    StorageBin,
                    CurrentStockParam>(
                    "CurrentStock_GetByWarehouseCode",
                    (cs, product, uom, warehouse, bin) =>
                    {
                        cs.Product = product;
                        cs.UoM = uom;
                        cs.Warehouse = warehouse;
                        cs.StorageBin = bin;
                        return cs;
                    },
                    new { WarehouseCode = warehouseCode },
                    splitOn: "ProductCode, UoMCode, WarehouseCode, StorageBinCode",
                    commandType: CommandType.StoredProcedure
                )).ToList();


                // STEP 2: Get Product list
                var productCodes = string.Join(",", stockList.Select(s => $"{s.Product.ProductCode}").Distinct());

                var products = (await conn.QueryAsync<ProductParam, VehicleModel, ProductCategory, ProductType, Brand, UnitOfMeasure, Dimension, ProductParam>(
                    "Product_GetByCode",
                    (product, model, category, type, brand, uom, dimension) =>
                    {
                        product.VehicleModel = model;
                        product.ProductCategory = category;
                        product.ProductType = type;
                        product.Brand = brand;
                        product.UnitOfMeasure = uom;
                        product.Dimension = dimension;

                        return product;
                    },
                    splitOn: "ID, ID, ID, ID, ID, ID",
                    param: new { ProductCode = productCodes },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                // STEP 3: Get only variants that exist in CurrentStock
                var variants = (await conn.QueryAsync<VariantParam>(
                    "ProductVariant_GetByStock",
                    new { WarehouseCode = warehouseCode },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                // Attach Product + Variant to stock
                foreach (var stock in stockList)
                {
                    var product = products.FirstOrDefault(p => p.ProductCode == stock.Product.ProductCode);
                    if (product != null)
                    {
                        product.VariantParams = variants
                            .Where(v => v.RefProductCode == product.ProductCode)
                            .ToList();

                        stock.Product = product;
                    }
                }

                result.Code = "0";
                result.Data = stockList;
               
            }
        }
        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = ex.Message;
        }

        return result;
    }
}
