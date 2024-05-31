using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models
{
    public class UserAddress
    {
        public Guid Id { get; set; }

        [Required]
        public int Number { get; set; }

        public string? Complement { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("AddressId")]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;

        public UserAddress(int userId, int addressId)
        {
            UserId = userId;
            AddressId = addressId;
        }

        public UserAddress(int userId, int addressId, int number, string? complement)
        {
            UserId = userId;
            AddressId = addressId;
            Number = number;
            Complement = complement;
        }

        public UserAddress()
        {
            
        }
    }
}
