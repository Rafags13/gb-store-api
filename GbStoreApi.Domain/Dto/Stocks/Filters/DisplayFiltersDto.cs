namespace GbStoreApi.Domain.Dto.Stocks.Filters
{
    public class DisplayFiltersDto
    {
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Brands { get; set; } = new List<string>();
        public List<string> Sizes { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
    }
}
