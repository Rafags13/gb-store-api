using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models.Purchases
{
    public class StorePickupPurchase
    {
        public Guid Id { get; set; }

        [ForeignKey("Address")]
        public int StoreAddressId { get; set; }
        public Address StoreAddress { get; set; } = null!;

        [ForeignKey("User")]
        public int UserBuyerId { get; set; }
        public User UserBuyer { get; set; } = null!;

        [ForeignKey("Purchase")]
        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; } = null!;
    }
}
