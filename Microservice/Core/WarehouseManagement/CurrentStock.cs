using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
namespace Core.WarehouseManagement
{
    public class CurrentStock : BaseClass.BaseClass
    {
        public string CurrentStockCode = "";
        public int ProductID { get; set; } = 0;
        public int? ProductVariantID { get; set; }

        public int UoMID { get; set; } = 0;

        public decimal Quantity { get; set; } = 0;

        public int WarehouseID { get; set; } = 0;

        public int? StorageBinID { get; set; }

    }

public class UDTT_CurrentStock
    {
        public string CurrentStockCode { get; set; } = string.Empty;

        public int ProductID { get; set; }

        public int? ProductVariantID { get; set; }

        public int? UoMID { get; set; }

        public decimal? Quantity { get; set; }

        public int? WarehouseID { get; set; }

        public int? StorageBinID { get; set; }

        public int ID { get; set; }

        public Guid RowPointer { get; set; } = Guid.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
