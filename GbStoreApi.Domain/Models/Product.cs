﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;


        [StringLength(200)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitaryPrice { get; set; }

        [Range(0, 1)]
        [DefaultValue(0)]
        public float? DiscountPercent { get; set; }

        [Range(1, 24)]
        [DefaultValue(1)]
        public int? QuotasNumber { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        #region [One to One]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        #endregion

        #region [One to Many]
        public virtual List<ProductStock> Stocks { get; set; }
        public virtual List<Picture> Pictures { get; set; }
        #endregion

        #region [Not Mapped]
        [NotMapped]
        public decimal PriceWithDiscount { get {
                return UnitaryPrice / (decimal)(1 - DiscountPercent ?? 0);
            }}
        #endregion

        public Product()
        {
            Stocks = new List<ProductStock>();
            Pictures = new List<Picture>();
        }
    }
}
