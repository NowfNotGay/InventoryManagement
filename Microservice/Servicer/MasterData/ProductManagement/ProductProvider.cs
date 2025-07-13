using Base.BaseService;
using Base.ProductManagement;
using Core.BaseClass;
using Core.MasterData.ProductClassification;
using Core.MasterData.ProductManagement;
using Core.MasterData.ProductProperties;
using Core.ProductManagement;
using Dapper;
using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Servicer.MasterData.ProductManagement;
public class ProductProvider : ICRUD_Service<Product, int>, IProductProvider
{
    private readonly IConfiguration _configuration;
    private readonly CloudDinaryHelper _cloudDinaryHelper;
    private readonly string _dapperConnectionString = string.Empty;
    private const int TimeoutInSeconds = 240;

    public ProductProvider(IConfiguration configuration, CloudDinaryHelper cloudDinaryHelper)
    {
        _configuration = configuration;
        _dapperConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory_DAPPER"));
        _cloudDinaryHelper = cloudDinaryHelper;
    }

    public Task<ResultService<Product>> Create(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<string>> Delete(int id)
    {
        throw new NotImplementedException();
    }




    #region DAPPER CRUD
    //public async Task<ResultService<Product>> SaveByDapper(Product entity)
    //{
    //    ResultService<Product> result = new();
    //    if (entity == null)
    //    {
    //        result.Code = "-1";
    //        result.Data = null;
    //        return result;
    //    }
    //    try
    //    {
    //        string Message = string.Empty;
    //        entity.RowPointer = Guid.Empty;
    //        entity.ProductCode = !entity.ProductCode.Contains("PRO") ? string.Empty : entity.ProductCode;
    //        List<Product> list = new();
    //        list.Add(entity);
    //        DataTable data = General.ConvertToDataTable(list);

    //        using (var connection = new SqlConnection(_dapperConnectionString))
    //        {
    //            await connection.OpenAsync();
    //            var param = new DynamicParameters();
    //            param.Add("@CreatedBy", entity.CreatedBy);
    //            param.Add("@udtt_Product", data.AsTableValuedParameter("UDTT_Product"));
    //            param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

    //            await connection.QueryAsync<Product>("Product_Save",
    //                param,
    //                commandType: CommandType.StoredProcedure,
    //                commandTimeout: TimeoutInSeconds);
    //            var resultMessage = param.Get<string>("@Message");
    //            if (resultMessage.Contains("successfully"))
    //            {
    //                result.Code = "0";
    //                result.Message = "Save Successfully";
    //            }
    //            else
    //            {
    //                result.Code = "-1";
    //                result.Message = "Failed";
    //            }
    //            return result;
    //        }
    //    }
    //    catch (SqlException sqlex)
    //    {

    //        result.Code = "2";
    //        result.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
    //        return result;
    //    }
    //    catch (DbUpdateConcurrencyException ex)
    //    {

    //        result.Code = "3";
    //        result.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
    //        return result;
    //    }
    //    catch (DbUpdateException ex)
    //    {

    //        result.Code = "4";
    //        result.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
    //        return result;
    //    }
    //    catch (OperationCanceledException ex)
    //    {

    //        result.Code = "5";
    //        result.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        result.Code = "6";
    //        result.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
    //        return result;
    //    }
    //}


    public async Task<ResultService<string>> DeleteByDapper(string code)
    {
        ResultService<string> result = new();
        if (string.IsNullOrEmpty(code))
        {
            result.Code = "-1";
            result.Message = "Entity is null";
            return result;
        }
        try
        {
            string Message = string.Empty;
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@ProductCode", code);
                param.Add("@Message", Message, dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                await connection.QueryAsync<string>("Product_Delete", param, commandType: CommandType.StoredProcedure, commandTimeout: TimeoutInSeconds);
                //Output Message từ Procedure
                var resultMessage = param.Get<string>("@Message");
                if (resultMessage.ToLower().Contains("successfully"))
                {
                    result.Code = "0";
                    result.Message = resultMessage;
                    result.Data = code;
                }
                else
                {
                    result.Code = "-1";
                    result.Message = resultMessage;
                    result.Data = string.Empty;
                }
                return result;
            }
        }
        catch (SqlException sqlex)
        {
            result.Code = "2";
            result.Message = $"Something wrong happened with Database, please Check the configuration: {sqlex.GetType()} - {sqlex.Message}";
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            result.Code = "3";
            result.Message = $"Concurrency error or Conflict happened : {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (DbUpdateException ex)
        {
            result.Code = "4";
            result.Message = $"Database update error: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (OperationCanceledException ex)
        {
            result.Code = "5";
            result.Message = $"Operation canceled: {ex.GetType()} - {ex.Message}";
            return result;
        }
        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
            return result;
        }

    }




    public async Task<ResultService<ProductParam>> Save(ProductSave entity)
    {
        ResultService<ProductParam> result = new();
        if (entity == null)
        {
            result.Code = "-1";
            result.Message = "Please fill in all Product's information";
            return result;
        }
        //if (entity.ImageFiles.IsNullOrEmpty())
        //{
        //    result.Code = "-1";
        //    result.Message = "Please choose Pictures";
        //    return result;
        //}

        ////Save ảnh lên Cloudinary

        //for (int i = 0; i < entity.ImageFiles.Count; i++)
        //{
        //    var variant = entity.VariantParams[i];
        //    var file = entity.ImageFiles[i].ImageFile;

        //    if (string.IsNullOrWhiteSpace(variant.ImageCode))
        //    {
        //        var (url, publicId) = await _cloudDinaryHelper.UploadImageAsync(file, null, "Product");

        //        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(publicId))
        //        {
        //            result.Code = "-1";
        //            result.Message = $"Upload image failed at index {i}";
        //            return result;
        //        }

        //        variant.ImageCode = publicId;
        //        variant.ImagePath = url;

        //        entity.Product.ImagePath = entity.ImageFiles[i].IsPrimary ? url : "";
        //    }

        //    else if (file != null && file.Length > 0)
        //    {
        //        var (url, publicId) = await _cloudDinaryHelper.UploadImageAsync(file, variant.ImageCode, "Product");

        //        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(publicId))
        //        {
        //            result.Code = "-1";
        //            result.Message = $"Overwrite image failed at index {i}";
        //            return result;
        //        }

        //        variant.ImagePath = url;

        //        entity.Product.ImagePath = entity.ImageFiles[i].IsPrimary ? url : "";
        //    }


        //}



        try
        {
            string Message = string.Empty;
            List<Product> Products = new();
            Products.Add(entity.Product);
            List<Dimension> Dimensions = new();
            Dimensions.Add(entity.Dimension);
            DataTable productData = General.ConvertToDataTable(Products);
            DataTable dimensionData = General.ConvertToDataTable(Dimensions);
            DataTable variantData = General.ConvertToDataTable(entity.VariantParams);

            using (var conn = new SqlConnection(_dapperConnectionString))
            {
                await conn.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@CreatedBy", entity.Product.CreatedBy);
                param.Add("@udtt_Product", productData.AsTableValuedParameter("UDTT_Product"));
                param.Add("@udtt_Variants", variantData.AsTableValuedParameter("UDTT_VariantParam"));
                param.Add("@udtt_Dimensions", dimensionData.AsTableValuedParameter("UDTT_Dimension"));
                param.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                param.Add("@No_", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                await conn.QueryAsync(
                    "Product_Save",
                    param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: TimeoutInSeconds
                    );
                Message = param.Get<string>("@Message");
                if (Message.Contains("successfully"))
                {
                    result.Code = "0";
                    result.Message = Message;
                    string productCode = param.Get<string>("@No_");
                    var newData = await GetByCodeParam(productCode);
                    result.Data = newData.Data;
                }
                else
                {
                    result.Code = "-1";
                    result.Message = Message;
                }
            }

        }

        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = $"An unexpected error occurred: {ex.GetType()} - {ex.Message}";
            return result;
        }
        return result;
    }

    public async Task<ResultService<IEnumerable<ProductParam>>> GetAllProductParam()
    {
        ResultService<IEnumerable<ProductParam>> result = new();
        try
        {
            using (var conn = new SqlConnection(_dapperConnectionString))
            {
                await conn.OpenAsync();
                result.Data = (await conn.QueryAsync<ProductParam, VehicleModel, ProductCategory, ProductType, Brand, UnitOfMeasure, Dimension, ProductParam>( 
                    "Product_GetAll",
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
                    splitOn: "VehicleModelID,ProductCategoryID,ProductTypeID,BrandID,UnitOfMeasureID,DimensionID",
                    commandType: CommandType.StoredProcedure
                )).ToList();
                //Cấu trúc Query trên:
                //QueryAsync < T1, T2, ..., TReturn > (
                //    string sql,
                //    Func < T1, T2, ..., TReturn > mapFunc,
                //    string splitOn = "ColumnX, ColumnY,..."
                //  )

                if (result.Data == null)
                {
                    result.Code = "-1";
                    result.Message = "Failed to load Products";
                    return result;
                }

                //ép chuỗi danh sách product code
                var productCodes = string.Join(",", result.Data.Select(p => $"{p.ProductCode}"));

                //LẤY variant từ danh sách code đã ép thành chuỗi
                var variants = (await conn.QueryAsync<VariantParam>(
                    "ProductVariant_GetByProductCodes",
                    new { ProductCodes = productCodes },
                    commandType: CommandType.StoredProcedure
                )).ToList();
                //Lấy ảnh phụ
                var images = (await conn.QueryAsync<ProductImages>(
                    "ProductImage_GetByProductCode",
                    new { ProductCodes = productCodes },
                    commandType: CommandType.StoredProcedure
                )).ToList();


                //gắn variant và images vào data trả về
                foreach (var product in result.Data)
                {
                    product.VariantParams = variants
                        .Where(v => v.RefProductCode.Equals(product.ProductCode))
                        .ToList();
                    product.ProductImages = images
                        .Where(pi => pi.RefProductCode.Equals(product.ProductCode))
                        .ToList();
                }

               

                result.Code = "0";
                result.Message = "Successfull";
            }
        }
        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = ex.Message;
        }
        return result;
    }

    public async Task<ResultService<ProductParam>> GetByCodeParam(string code)
    {
        ResultService<ProductParam> result = new();
        try
        {
            using (var conn = new SqlConnection(_dapperConnectionString))
            {
                await conn.OpenAsync();
                var list = await conn.QueryAsync<ProductParam, VehicleModel, ProductCategory, ProductType, Brand, UnitOfMeasure, Dimension, ProductParam>(
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
                     splitOn: "VehicleModelID,ProductCategoryID,ProductTypeID,BrandID,UnitOfMeasureID,DimensionID",
                     param: new { ProductCode = code },
                     commandType: CommandType.StoredProcedure
                 );

                result.Data = list.FirstOrDefault()!;
               

                if (result.Data == null)
                {
                    result.Code = "-1";
                    result.Message = "Failed to load Products";
                    return result;
                }

                //LẤY variant từ  code 
                result.Data.VariantParams = (await conn.QueryAsync<VariantParam>(
                    "ProductVariant_GetByProductCodes",
                    new { ProductCodes = result.Data.ProductCode },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                result.Data.ProductImages= (await conn.QueryAsync<ProductImages>(
                    "ProductImage_GetByProductCode",
                    new { ProductCodes = result.Data.ProductCode },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                result.Code = "0";
                result.Message = "Successfull";
            }
        }
        catch (Exception ex)
        {
            result.Code = "6";
            result.Message = ex.Message;
        }
        return result;
    }

    public Task<ResultService<Product>> Update(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<Product>> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<IEnumerable<Product>>> GetAll()
    {
        throw new NotImplementedException();
    }
    #endregion
}
