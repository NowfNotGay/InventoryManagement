using Core.MasterData.ProductClassification;
using Core.MasterData.ProductManagement;
using Core.MasterData.ProductProperties;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductManagement;
public class Product : BaseClass.BaseClass
{
    public string? ProductCode { get; set; }
    public string ProductName { get; set; }
    public int ModelID { get; set; }
    public int CategoryID { get; set; }
    public int TypeID { get; set; }
    public int BrandID { get; set; }
    public int UoMID { get; set; }

    public string Description { get; set; }
    public string? PublicImgID { get; set; } = null;
    public string? ImagePath { get; set; }
    public double PurchasePrice { get; set; }
    public double SalePrice { get; set; }
}



public class ProductSave
{
    public Product Product { get; set; }

    public Dimension Dimension { get; set; }

    public List<VariantParam> VariantParams { get; set; }

    public IFormFile? ProductImg { get; set; }


    public List<ImageFileDTO>? ImageFiles { get; set; }

 
    public List<ImageFileDTO>? VariantImgs { get; set; }
}

public class ImageFileDTO
{
    public IFormFile? ImageFile { get; set; } = null;
    public bool IsPrimary { get; set; }
    public int Position { get; set; }
}

public class VariantParam
{
    public string? ProductVariantCode { get; set; }
    public string? ImageCode { get; set; } = null;
    public string? AttributeCode { get; set; }
    public string? RefProductCode { get; set; }
    public int? Position { get; set; }
    public string? ImagePath { get; set; }
    public bool? IsPrimary { get; set; }

    public int ColorID { get; set; }
    public string? ColorName { get; set; }
    public int MaterialID { get; set; }
    public string? MaterialName { get; set; }
}


public class ProductParam : BaseClass.BaseClass
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }

    public string Description { get; set; }
    public string PublicImgID { get; set; }
    public string ImagePath { get; set; }
    public double PurchasePrice { get; set; }
    public double SalePrice { get; set; }

    //VehicalModel
    public VehicleModel VehicleModel { get; set; }

    //ProductCategory
    public ProductCategory ProductCategory { get; set; }
    //ProductType
    public ProductType ProductType { get; set; }

    //Brand
    public Brand Brand { get; set; }
    //Dimension
    public Dimension Dimension { get; set; }
    public string DimensionUoM { get; set; }


    //UoM
    public UnitOfMeasure UnitOfMeasure { get; set; }

    public List<VariantParam> VariantParams { get; set; }
    public List<ProductImages> ProductImages { get; set; }
}