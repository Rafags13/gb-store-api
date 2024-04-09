namespace GbStoreApi.Domain.Dto
{
    public class DisplayProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal RealPrice { get; set; }
        public decimal PriceWithDiscount { get; set; }
        public float? DiscountPercent { get; set; }
        public string PhotoUrlId { get; set; } = string.Empty;
        public IEnumerable<string>? VariantNames { get; set; } = Enumerable.Empty<string>();
    }
}
