namespace GbStoreApi.Domain.Dto.Product.Catalogs
{
    public class CatalogFilterDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string[]? Sizes { get; set; } = Array.Empty<string>();
        public string[]? Colors { get; set; } = Array.Empty<string>();
        public string? Category = string.Empty;
    }
}
