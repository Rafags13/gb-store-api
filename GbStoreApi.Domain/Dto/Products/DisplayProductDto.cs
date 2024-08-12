using Newtonsoft.Json;

namespace GbStoreApi.Domain.Dto.Products
{
    public class DisplayProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal RealPrice { get; set; }
        public decimal PriceWithDiscount { get; set; }
        public float? DiscountPercent { get; set; }
        public string PhotoUrlId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [JsonIgnore]
        public string Category { get; set; } = string.Empty;
        public IEnumerable<string>? Colors { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string>? Sizes { get; set; } = Enumerable.Empty<string>();
    }
}

