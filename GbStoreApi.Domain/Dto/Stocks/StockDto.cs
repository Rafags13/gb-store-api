namespace GbStoreApi.Domain.Dto.Stocks
{
    public class StockDto
    {
        public required int StockId { get; set; }
        public required string ColorName { get; set; }
        public required string SizeName { get; set; }
        public required int Amount { get; set; }
    }
}
