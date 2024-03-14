using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface IColorService
    {
        DisplayColorDto CreateColor(string colorName);
        IEnumerable<DisplayColorDto> GetAll();
        DisplayColorDto? GetByName(string colorName);
        DisplayColorDto? GetById(int id);
    }
}
