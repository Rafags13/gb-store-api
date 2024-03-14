using GbStoreApi.Domain.enums;

namespace GbStoreApi.Domain.Dto
{
    public class SignUpDto
    {
        public string Fullname { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime BirthdayDate { get; set; }
        public UserType TypeOfUser { get; set; } = UserType.Common;
    }
}
