namespace GbStoreApi.Domain.Dto.Product.Catalogs
{
    public class CatalogFilterDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string[]? Tamanhos { get; set; } = Array.Empty<string>();
        public string[]? Cores { get; set; } = Array.Empty<string>();
        public string? Category = string.Empty;
    }
}
