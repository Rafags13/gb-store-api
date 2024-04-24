namespace GbStoreApi.Domain.Dto.Authentications
{
    public class SignInDto
    {
        public SignInDto() { }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
