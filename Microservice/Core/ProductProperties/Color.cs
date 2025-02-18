using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductProperties;
[Table("Color")]
public class Color : BaseClass.BaseClass
{
    public string ColorCode { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
}
