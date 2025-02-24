namespace Core.MasterData
{
    public class TransactionType : BaseClass.BaseClass
    {
        public string TransactionTypeCode { get; set; }
        public string TransactionTypeName { get; set; }
        public int DocumentTypeID { get; set; }
        public string? Description { get; set; }
    }
}
