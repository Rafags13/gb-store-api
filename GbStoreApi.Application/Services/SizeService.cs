using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Services
{
    public class SizeService : ISizeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SizeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public DisplaySizeDto Create(string sizeName)
        {
            var newSize = new Size { Name = sizeName };
            _unitOfWork.Size.Add(newSize);
            _unitOfWork.Save();

            var currentSize = _unitOfWork.Size.FindOne(x => x.Name == sizeName);
            var displaySize = new DisplaySizeDto { Id =  currentSize.Id, Name = currentSize.Name };

            return displaySize;
        }

        public IEnumerable<DisplaySizeDto> GetAll()
        {
            return _unitOfWork.Size.GetAll().Select(x => new DisplaySizeDto { Id = x.Id, Name = x.Name});
        }

        public DisplaySizeDto? GetById(int id)
        {
            var currentSize = _unitOfWork.Size.GetById(id);
            if (currentSize == null) return null;

            var sizeDisplay = new DisplaySizeDto { Id = currentSize.Id, Name = currentSize.Name };

            return sizeDisplay;
        }

        public DisplaySizeDto? GetByName(string sizeName)
        {
            var currentSize = _unitOfWork.Size.FindOne(x => x.Name == sizeName);
            if (currentSize == null) return null;

            var sizeDisplay = new DisplaySizeDto { Id = currentSize.Id, Name = currentSize.Name };

            return sizeDisplay;
        }
    }
}
