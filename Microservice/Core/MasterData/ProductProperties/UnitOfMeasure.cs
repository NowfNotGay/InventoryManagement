using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MasterData.ProductProperties;
[Table("UnitOfMeasure")]
public class UnitOfMeasure : BaseClass.BaseClass
{
    public string UoMCode { get; set; } = string.Empty;
    public string UoMName { get; set; } = string.Empty;
    public string UoMDescription { get; set; } = string.Empty;
}
