using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.MasterData;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Core.WarehouseManagement
{
    public class GoodsReceiptNote : BaseClass.BaseClass
    {
        public string GRNCode { get; set; }
        public int WarehouseID { get; set; }
        public int SupplierID { get; set; }
        public int TransactionTypeID { get; set; }
        public DateTime ReceiptDate { get; set; } = DateTime.Now;
        public string Notes { get; set; }
    }
}
