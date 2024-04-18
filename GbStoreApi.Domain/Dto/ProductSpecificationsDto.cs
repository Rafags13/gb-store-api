using GbStoreApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Dto
{
    public class ProductSpecificationsDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Category { get; set; }
        public required IEnumerable<StockDto> Stocks { get; set; }
        public required decimal RealPrice { get; set; }
        public required decimal PriceWithDiscount { get; set; }
        public required IEnumerable<string> ProductPictureIds { get; set; }
    }
}
