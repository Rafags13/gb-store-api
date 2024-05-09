using GbStoreApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
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

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal FinalPrice { get; set; }

        public virtual Address DeliveryAddress { get; set; }

        public string? DeliveryInstructions { get; set; }

        public DeliveryType TypeOfDelivery { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }

    }
}
