namespace Core.TransactionManagement
{
    public class GoodsReceiptNote : BaseClass.BaseClass
    {
        public string GRNCode { get; set; }
        public string WarehouseCode { get; set; }
        public string SupplierCode { get; set; }
        public string TransactionTypeCode { get; set; }
        public DateTime ReceiptDate { get; set; } = DateTime.Now;
        public string Notes { get; set; }
    }

    public class GoodsReceiptNoteLine : BaseClass.BaseClass
    {
        public string RefGRNCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductVariantCode { get; set; }
        public string UoMCode { get; set; }
        public decimal Quantity { get; set; }
        public string UoMConversionCode { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public string StorageBinCode { get; set; }
    }

    public class GoodsReceiptNote_Param
    {
        public string CreatedBy { get; set; }
        public GoodsReceiptNote GRNs { get; set; }
        public List<GoodsReceiptNoteLine> GRNLines { get; set; }
    }
}
