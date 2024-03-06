using GbStoreApi.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
