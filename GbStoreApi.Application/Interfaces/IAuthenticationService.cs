using GbStoreApi.Domain.Dto.Authentications;

namespace GbStoreApi.Application.Interfaces
{
    public interface IAuthenticationService
    {
        string? SignIn(SignInDto signInDto);
        string SignUp(SignUpDto signUpDto);
        string RefreshToken(int subUserId);
    }
}
