using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MasterData.ProductProperties;
public class Dimension : BaseClass.BaseClass
{
    public string ProductCode { get; set; }
    public decimal? Height { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public string UoMHeightCode { get; set; }
    public string UoMLengthCode { get; set; }
    public string UoMWidthCode { get; set; }
}
