using GbStoreApi.Domain.Enums;

namespace GbStoreApi.Domain.Dto.Purchases
{
    public class AdminPurchaseSpecificationDto
    {
        public int PurchaseId { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public decimal FinalPrice { get; set; }
        public IEnumerable<DisplayPurchaseItem> OrderItems { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DeliveryType TypeOfDelivery { get; set; }
        public PaymentMethod TypeOfPayment { get; set; }
        public PurchaseState PurchaseState { get; set; }

        public AdminPurchaseSpecificationDto()
        {
            OrderItems = Enumerable.Empty<DisplayPurchaseItem>();
        }
    }
}
