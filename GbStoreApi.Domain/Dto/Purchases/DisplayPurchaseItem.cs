namespace GbStoreApi.Domain.Dto.Purchases
{
    public class DisplayPurchaseItem
    {
        public required string ProductName { get; set; }
        public required string ProductUrl { get; set; }
        public int Amount { get; set; }
        public decimal UnitaryPrice { get; set; }
    }
}
