using System.Text.Json.Serialization;

namespace GbStoreApi.Domain.Dto.Address
{
    public class DisplayAddressDto : BaseAddressDto
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public DisplayAddressDto() : base()
        {
            
        }
    }
}
