namespace GbStoreApi.Domain.Dto.Products
{
    public class ExistentQueryDto
    {
        public string? SearchQuery;
        public string? Name;
        public string? Price;
        public string? Category;
        public string? Brand;
        public int Page = 0;
        public int PageSize = 20;
    }
}
