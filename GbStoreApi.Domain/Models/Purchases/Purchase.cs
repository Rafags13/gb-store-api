using GbStoreApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models.Purchases
{
    public class Purchase
    {
        #region [Common Data]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal FinalPrice { get; set; }

        public PaymentMethod TypeOfPayment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.Now;
        #endregion

        #region [Types of Purchase]
        public DeliveryType TypeOfDelivery { get; set; }
        public virtual ShippingPurchase? ShippingPurchase { get; set; }
        public virtual StorePickupPurchase? StorePickupPurchase { get; set; }
        #endregion

        public virtual ICollection<OrderItems> OrderItems { get; set; }

        public Purchase()
        {
            OrderItems = new List<OrderItems>();
        }
    }
}
