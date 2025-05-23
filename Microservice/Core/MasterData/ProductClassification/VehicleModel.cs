using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MasterData.ProductClassification;
[Table("VehicleModel")]

public class VehicleModel : BaseClass.BaseClass
{
    public string ModelCode { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string BrandCode { get; set; }
}

