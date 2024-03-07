﻿using System.Xml.Linq;

namespace GbStoreApi.Domain.Dto
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
        public IEnumerable<CreateStockDto> Stocks { get; set; } = Enumerable.Empty<CreateStockDto>();
    }
}
