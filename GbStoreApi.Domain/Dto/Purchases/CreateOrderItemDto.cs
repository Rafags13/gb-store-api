namespace GbStoreApi.Domain.Dto.Purchases
{
    public class CreateOrderItemDto
    {
        public int ProductStockId { get; set; }
        public int ProductCount { get; set; }
        public decimal ProductStockPrice { get; set; }
    }
}
