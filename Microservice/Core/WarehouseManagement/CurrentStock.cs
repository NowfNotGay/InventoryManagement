using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.WarehouseManagement;
public class CurrentStock : BaseClass.BaseClass
{
    public string CurrentStockCode { get; set; }
    public string ProductCode { get; set; } 
    public string ProductVariantCode { get; set; }
    public string UoMCode { get; set; }
    public decimal Quantity { get; set; }
    public int WarehouseCode { get; set; }
    public int StorageBinCode { get; set; }
}

