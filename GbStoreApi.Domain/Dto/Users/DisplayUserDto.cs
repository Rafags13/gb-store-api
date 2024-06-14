using GbStoreApi.Domain.enums;

namespace GbStoreApi.Domain.Dto.Users
{
    public class DisplayUserDto
    {
        public DisplayUserDto() { }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthdayDate { get; set; }
        public string TypeOfUser { get; set; }
    }
}
