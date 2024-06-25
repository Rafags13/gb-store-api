using GbStoreApi.Domain.Enums;

namespace GbStoreApi.Domain.Dto.Purchases
{
    public class AdminPurchaseDisplay
    {
        public int Id { get; set; }
        public required string Photo { get; set; }
        public required string BoughterName { get; set; }
        public decimal Price { get; set; }
        public DeliveryType TypeOfPayment { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
    }
}
