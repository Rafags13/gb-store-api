using System.ComponentModel.DataAnnotations.Schema;

namespace GbStoreApi.Domain.Models
{
    public class UserAddress
    {
        public Guid Id { get; set; }

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
    }
}
