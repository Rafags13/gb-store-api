namespace GbStoreApi.Domain.Dto.Address
{
    public class CreateAddressDto
    {
        public required string ZipCode { get; set; }
        public required string StreetName { get; set; }
        public required string Neighbourhood { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public int Number { get; set; }
        public string? Complement { get; set; }
    }
}
