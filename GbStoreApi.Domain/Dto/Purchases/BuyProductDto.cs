using GbStoreApi.Domain.Enums;

namespace GbStoreApi.Domain.Dto.Purchases
{
    public class BuyProductDto : BaseBuyProductDto
    {
        public string? SelectedZipCode { get; set; }
        public string? DeliveryInstructions { get; set; }

        public BuyProductDto()
        {
            Items = new List<CreateOrderItemDto>();
        }
    }
}
