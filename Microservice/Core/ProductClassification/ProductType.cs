using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductClassification;
[Table("ProductType")]
public class ProductType : BaseClass.BaseClass
{
    public string ProductTypeCode { get; set; } = string.Empty;
    public string ProductTypeName { get; set; } = string.Empty;
}
