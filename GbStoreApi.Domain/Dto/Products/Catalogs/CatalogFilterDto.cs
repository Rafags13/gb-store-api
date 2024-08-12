using GbStoreApi.Domain.Enums;

namespace GbStoreApi.Domain.Dto.Product.Catalogs
{
    public class CatalogFilterDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string[]? Sizes { get; set; } = Array.Empty<string>();
        public string[]? Colors { get; set; } = Array.Empty<string>();
        public string? OrderBy { get; set; }
        public string? Direction { get; set; }
        public string? Category { get; set; }
        public string? ProductName { get; set; }


        public void Deconstruct(
            out int Page,
            out int PageSize,
            out string[]? Sizes,
            out string[]? Colors,
            out string? OrderBy,
            out string? Direction,
            out string? ProductName,
            out string? Category)
        {
            Page = this.Page;
            PageSize = this.PageSize;
            Sizes = this.Sizes;
            Colors = this.Colors;
            OrderBy = this.OrderBy;
            Direction = this.Direction;
            Category = this.Category;
            ProductName = this.ProductName;
        }
    }
}
