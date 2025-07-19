namespace Core.ProductManagement
{
    public class ProductVariant : BaseClass.BaseClass
    {
        public string ProductCode { get; set; }
        public string ProductVariantCode { get; set; }
        public string? Attributes { get; set; }
    }
}
