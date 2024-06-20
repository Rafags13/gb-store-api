using GbStoreApi.Domain.Dto.Stocks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GbStoreApi.Domain.Dto.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal UnitaryPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public int? QuotasNumber { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string StockSerialized { get; set; } = string.Empty;
        public List<CreateStockDto>? Stock
        {
            get
            {
                return JsonConvert.DeserializeObject<List<CreateStockDto>>(StockSerialized);
            }
        }
        public List<IFormFile> Photos { get; set; } = null!;
    }
}
