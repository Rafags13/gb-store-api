using GbStoreApi.Domain.Enums;
using Newtonsoft.Json;

namespace GbStoreApi.Domain.Dto.Purchases
{
    public class PurchaseSpecificationDto
    {
        [JsonIgnore]
        public int BoughterId {get;set;} 
        public int PurchaseId { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public decimal FinalPrice { get; set; }
        public IEnumerable<DisplayPurchaseItem> OrderItems { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DeliveryType TypeOfDelivery { get; set; }
        public PaymentMethod TypeOfPayment { get; set; }

        public PurchaseSpecificationDto()
        {
            OrderItems = Enumerable.Empty<DisplayPurchaseItem>();
        }
    }
}
