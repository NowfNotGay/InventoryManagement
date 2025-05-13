using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductManagement;
[Table("ProductUoMConversion")]
public class ProductUoMConversion : BaseClass.BaseClass
{
    public string ProductUoMConversionCode { get; set; } = string.Empty;
    public int ProductID { get; set; }
    public int FromUoMID { get; set; }
    public int ToUoMID { get; set; }
    public decimal ConversionRate{ get; set; }
}

