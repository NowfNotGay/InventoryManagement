using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.WarehouseManagement;
[Table("Warehouse")]
public class Warehouse : BaseClass.BaseClass
{
    public string WarehouseCode { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public bool AllowNegativeStock { get; set; } = false;
    public string Address { get; set; } = string.Empty;
    public int? BinLocationCount { get; set; }


}
