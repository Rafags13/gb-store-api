namespace GbStoreApi.Domain.Dto.Stocks
{
    [Serializable()]
    public class CreateStockDto
    {
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int StockSize { get; set; }
    }
}
