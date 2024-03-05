using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<DisplayUserDto> GetAll();
        DisplayUserDto? GetById(int id);
        DisplayUserDto? GetCurrentInformations();
    }
}
