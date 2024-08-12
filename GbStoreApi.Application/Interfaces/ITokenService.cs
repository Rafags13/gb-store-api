using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Users;

namespace GbStoreApi.Application.Interfaces
{
    public interface ITokenService
    {
        string Generate(UserTokenDto user);
        RefreshToken GenerateRefresh();
    }
}
