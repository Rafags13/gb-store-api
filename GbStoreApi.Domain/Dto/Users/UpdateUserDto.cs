namespace GbStoreApi.Domain.Dto.Users
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthdayDate { get; set; }
    }
}
