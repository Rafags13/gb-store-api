using GbStoreApi.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Interfaces
{
    public interface IBrandService
    {
        IEnumerable<DisplayBrandDto> GetAll();
        DisplayBrandDto? GetById(int id);
        DisplayBrandDto? GetByName(string brandName);
        DisplayBrandDto Create(string brandName);
    }
}
