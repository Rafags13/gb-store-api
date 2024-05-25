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
        public virtual Address DeliveryAddress { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User Buyer { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal FinalPrice { get; set; }

        public string? DeliveryInstructions { get; set; }

        public DeliveryType TypeOfDelivery { get; set; }

        public PaymentMethod TypeOfPayment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.Now;

        public virtual ICollection<OrderItems> OrderItems { get; set; }

        public Purchase()
        {
            OrderItems = new List<OrderItems>();
        }
    }
}
