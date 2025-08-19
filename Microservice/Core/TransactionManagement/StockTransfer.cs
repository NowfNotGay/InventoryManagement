namespace Core.TransactionManagement
{
    public class StockTransfer : BaseClass.BaseClass
    {
        public string TransferCode { get; set; }
        public string FromWarehouseCode { get; set; }
        public string ToWarehouseCode { get; set; }
        public string TransactionTypeCode { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
    }

    public class StockTransferDetail : BaseClass.BaseClass
    {
        public string StockTransferCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductVariantCode { get; set; }
        public string UoMCode { get; set; }
        public decimal Quantity { get; set; }
        public string UoMConversionCode { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public string FromStorageBinCode { get; set; }
        public string ToStorageBinCode { get; set; }
    }

    public class StockTransfer_Param
    {
        public string CreatedBy { get; set; }
        public List<StockTransfer> StockTransfers { get; set; }
        public List<StockTransferDetail> StockTransferDetails { get; set; }
    }
}
