using GbStoreApi.Domain.Enums;

namespace GbStoreApi.Domain.Dto.Purchases
{
    public class BuyProductDto
    {
        public required string ZipCode { get; set; }
        public IEnumerable<CreateOrderItemDto> Items { get; set; }
        public string? DeliveryInstructions { get; set; }
        public DeliveryType TypeOfDelivery { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public BuyProductDto()
        {
            Items = new List<CreateOrderItemDto>();
        }
    }
}
