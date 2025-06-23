using Core.MasterData.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductManagement;
public class Product : BaseClass.BaseClass
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public int ModelID { get; set; }
    public int CategoryID { get; set; }
    public int TypeID { get; set; }
    public int BrandID { get; set; }
    public int DemensionID { get; set; }
    public int UoMID { get; set; }

    public string Description { get; set; }
    public string ImagePath { get; set; }
    public double PurchasePrice { get; set; }
    public double SalePrice { get; set; }
}

public class ProductParam : BaseClass.BaseClass
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }

    //VehicalModel
    public string ModelCode { get; set; }
    public string ModelName { get; set; }

    //ProductCategory
    public string CategoryNCode { get; set; }
    public string CategoryName { get; set; }
    //ProductType
    public string TypeCode{ get; set; }
    public string TypeName { get; set; }

    //Brand
    public string BrandCode { get; set; }
    public string BrandName { get; set; }

    //Dimension
    public decimal? Height { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public string UoMHeightCode { get; set; }
    public string UoMLengthCode { get; set; }
    public string UoMWidthCode { get; set; }


    //UoM
    public string UoMCode { get; set; }
    public string UoMName { get; set; }
    public string UoMDescription { get; set; }


    public string Description { get; set; }
    public string ImagePath { get; set; }
    public double PurchasePrice { get; set; }
    public double SalePrice { get; set; }

    List<ProductVariant> ProductVariants { get; set; }
}

public class ProductSave
{
    public Product Product {  get; set; }

    List<VariantParam> VariantParams { get; set; }
}

public class VariantParam
{

    public string ImageCode { get; set; }
    public string AttributeCode { get; set; }
    public string RefProductCode { get; set; }
    public int Position { get; set; }
    public string ImagePath { get; set; }
    public bool IsPrimary { get; set; }

    public int ColorID { get; set; }
    public int MaterialID { get; set; }
}