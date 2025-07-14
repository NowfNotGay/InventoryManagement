using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MasterData.ProductManagement;
public class ProductImages : BaseClass.BaseClass
{
    public string ImageCode { get; set; }
    public string ProductVariantCode { get; set; }
    public string RefProductCode { get; set; }
    public int Position { get; set; }
    public string ImagePath { get; set; }
    public bool IsPrimary { get; set; }
}
