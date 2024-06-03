using GbStoreApi.Domain.Enums;

namespace GbStoreApi.Domain.Dto.Purchases
{
    public class BaseBuyProductDto
    {
        public IEnumerable<CreateOrderItemDto> Items { get; set; }
        public DeliveryType TypeOfDelivery { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
