namespace Core.ProductManagement
{
    public class ProductVariant : BaseClass.BaseClass
    {
        public int ProductID { get; set; }
        public string ProductVariantCode { get; set; }
        public string? Attributes { get; set; }
    }
}
