using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models.Purchases
{
    public class ShippingPurchase
    {
        public Guid Id { get; set; }

        public string? DeliveryInstructions { get; set; }

        [ForeignKey("UserAddress")]
        public int UserAddressId { get; set; }
        public UserAddress UserOwnerAddress { get; set; } = null!;

        [ForeignKey("Purchase")]
        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; } = null!;
    }
}
