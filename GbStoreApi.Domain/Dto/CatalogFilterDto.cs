namespace GbStoreApi.Domain.Dto
{
    public class CatalogFilterDto
    {
        public string[]? Tamanhos { get; set; } = Array.Empty<string>();
        public string[]? Cores { get; set; } = Array.Empty<string>();
        public string? Category = string.Empty;
    }
}
