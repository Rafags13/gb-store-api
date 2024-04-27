using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface IAuthenticationService
    {
        ResponseDto<string> SignIn(SignInDto signInDto);
        ResponseDto<string> SignUp(SignUpDto signUpDto);
        string RefreshToken(int subUserId);
    }
}
