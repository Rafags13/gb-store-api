using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface ITokenService
    {
        UserTokenDto CreateModelByUser(User user);
        string Generate(UserTokenDto user);
        RefreshToken GenerateRefresh();
    }
}
