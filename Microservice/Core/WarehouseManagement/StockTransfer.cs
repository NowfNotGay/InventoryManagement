namespace Core.WarehouseManagement
{
    public class StockTransfer : BaseClass.BaseClass
    {
        public string TransferCode { get; set; }
        public int FromWarehouseID { get; set; }
        public int ToWarehouseID { get; set; }
        public int TransactionTypeID { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
    }

    public class StockTransferDetail : BaseClass.BaseClass
    {
        public int StockTransferID { get; set; }
        public int ProductID { get; set; }
        public int ProductVariantID { get; set; }
        public int UoMID { get; set; }
        public decimal Quantity { get; set; }
        public int UoMConversionID { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public int FromStorageBinID { get; set; }
        public int ToStorageBinID { get; set; }
    }

    public class StockTransfer_Param
    {
        public string CreatedBy { get; set; }
        public List<StockTransfer> StockTransfers { get; set; }
        public List<StockTransferDetail> StockTransferDetails { get; set; }
    }
}
