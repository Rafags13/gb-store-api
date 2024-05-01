﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string StreetName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Neighbourhood { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        public string State { get; set; } = string.Empty;

        [Required]
        public int Number { get; set; }

        public string? Complement { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User UserOwner { get; set; }
    }
}
