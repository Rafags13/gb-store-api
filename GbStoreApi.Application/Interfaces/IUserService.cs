using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.enums;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<DisplayUserDto> GetAll();
        DisplayUserDto? GetById(int id);
        DisplayUserDto? GetCurrentInformations();
        UserType? GetUserRole();
        User? GetByCredentials(SignInDto signInDto);
    }
}
