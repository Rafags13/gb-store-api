using GbStoreApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models
{
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Address")]
        public int DeliveryAddressId { get; set; }
        public virtual Address DeliveryAddress { get; set; }

        public string? DeliveryInstructions { get; set; }

        public DeliveryType TypeOfDelivery { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }

        [NotMapped]
        public decimal FinalPrice { get {
                var fullPrice =
                    OrderItems
                        .Select(x => new {
                            PriceByProduct = x.ProductCount * x.Stock.Product.UnitaryPrice})
                        .Sum(x => x.PriceByProduct);

                return fullPrice;
            }}
    }
}
