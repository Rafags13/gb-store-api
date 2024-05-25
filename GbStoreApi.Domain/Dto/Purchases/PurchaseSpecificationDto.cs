using GbStoreApi.Domain.Enums;
using Newtonsoft.Json;

namespace GbStoreApi.Domain.Dto.Purchases
{
    public class PurchaseSpecificationDto
    {
        [JsonIgnore]
        public int BoughterId {get;set;} 
        public string ZipCode { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ProductUrl { get; set; } = string.Empty;
        public DateTime BoughtDate { get; set; }
        public DeliveryType TypeOfDelivery { get; set; }
        public PaymentMethod TypeOfPayment { get; set; }
    }
}
