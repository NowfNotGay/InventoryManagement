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
        public int ProductID { get; set; } = 0;
        public int? ProductVariantID { get; set; }

        public int UoMID { get; set; } = 0;

        public decimal Quantity { get; set; } = 0;

        public int WarehouseID { get; set; } = 0;

        public int? StorageBinID { get; set; }

    }
    public class UDTT_CurrentStock
    {
        public string CreatedBy { get; set; } = "";
        public string CreatedDate { get; set; } = "";
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ProductVariantID { get; set; }
        public decimal Quantity { get; set; }
        public string RowPointer { get; set; } = "";
        public int StorageBinID { get; set; }
        public int UoMID { get; set; }
        public string UpdatedBy { get; set; } = "";
        public string UpdatedDate { get; set; } = "";
        public int WarehouseID { get; set; }
    }
}
