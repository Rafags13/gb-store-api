using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Categories;
using GbStoreApi.Domain.Dto.Stocks;

namespace GbStoreApi.Domain.Dto.Products
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal UnitaryPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public int? QuotasNumber { get; set; }
        public DisplayCategoryDto Category { get; set; } = null!;
        public DisplayBrandDto Brand { get; set; } = null!;
        public List<CreateStockDto> Stocks { get; set; }
        public List<string> Photos { get; set; }
    }
}
