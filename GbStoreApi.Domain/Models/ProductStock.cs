using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models
{
    public class ProductStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Count { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color? Color { get; set; }

        [ForeignKey("Size")]
        public int SizeId { get; set; }
        public Size? Size { get; set; }
    }
}
