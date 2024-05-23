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
        public string? Category = string.Empty;
    }
}
