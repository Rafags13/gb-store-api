using GbStoreApi.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
