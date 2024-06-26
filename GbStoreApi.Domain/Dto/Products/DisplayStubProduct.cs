namespace GbStoreApi.Domain.Dto.Products
{
    public class DisplayStubProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProductPictureId { get; set; } = string.Empty;
        public decimal PriceWithDiscount { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
    }
}
