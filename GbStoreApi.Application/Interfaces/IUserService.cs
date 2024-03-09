using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<DisplayUserDto> GetAll();
        DisplayUserDto? GetById(int id);
        DisplayUserDto? GetCurrentInformations();
        User GetByCredentials(SignInDto signInDto);
    }
}
