using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models
{
    public class OrderItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ProductStock")]
        public int ProductStockId { get; set; }
        public virtual ProductStock Stock { get; set; }
        public int ProductCount { get; set; }

        [ForeignKey("Purchase")]
        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}
