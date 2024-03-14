using GbStoreApi.Domain.enums;

namespace GbStoreApi.Domain.Dto
{
    public class DisplayUserDto
    {
        public DisplayUserDto() { }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserType TypeOfUser { get; set; }
    }
}
