using GbStoreApi.Domain.Dto.Sizes;

namespace GbStoreApi.Application.Interfaces
{
    public interface ISizeService
    {
        IEnumerable<DisplaySizeDto> GetAll();
        DisplaySizeDto? GetById(int id);
        DisplaySizeDto? GetByName(string sizeName);
        DisplaySizeDto Create(string sizeName);
    }
}
