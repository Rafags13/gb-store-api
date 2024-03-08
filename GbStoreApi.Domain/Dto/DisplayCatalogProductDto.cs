namespace GbStoreApi.Domain.Dto
{
    public class DisplayCatalogProductDto
    {
        public int Id { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
