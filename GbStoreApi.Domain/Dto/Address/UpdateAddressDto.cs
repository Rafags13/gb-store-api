namespace GbStoreApi.Domain.Dto.Address
{
    public class UpdateAddressDto
    {
        public string ZipCode { get; set; } = string.Empty;
        public int Number { get; set; }
        public string? Complement { get; set; }
    }
}
