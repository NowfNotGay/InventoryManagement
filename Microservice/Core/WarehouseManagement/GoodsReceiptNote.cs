using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.MasterData;
using Core.ProductManagement;
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

    public class GoodsReceiptNoteLine : BaseClass.BaseClass
    {
        public string RefGRNCode { get; set; }
        public int ProductID { get; set; }
        public int ProductVariantID { get; set; }
        public int UoMID { get; set; }
        public decimal Quantity { get; set; }
        public int UoMConversionID { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public int StorageBinID { get; set; }
    }

    public class GoodsReceiptNote_Param
    {
        public string CreatedBy { get; set; }
        public List<GoodsReceiptNote> GRNs { get; set; }
        public List<GoodsReceiptNoteLine> GRNLines { get; set; }
    }
}
