using Microsoft.AspNetCore.Http;

namespace GbStoreApi.Domain.Dto
{
    public class CreateProductDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal UnitaryPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public int? QuotasNumber { get; set; }
        public required int CategoryId { get; set; }
        public required int BrandId { get; set; }
        public IEnumerable<CreateStockDto> Stocks { get; set; } = Enumerable.Empty<CreateStockDto>();
        public IEnumerable<IFormFile> Files { get; set; } = Enumerable.Empty<IFormFile>();
    }
}
