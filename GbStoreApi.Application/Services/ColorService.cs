using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Application.Services
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ColorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public DisplayColorDto CreateColor(string colorName)
        {
            if(ColorAlsoExists(colorName))
            {
                throw new ArgumentException("A cor informada já existe no sistema.");
            }

            var newColor = new Color { Name = colorName};
            _unitOfWork.Color.Add(newColor);
            _unitOfWork.Save();

            var currentColor = GetByName(colorName);

            return currentColor;
        }

        private bool ColorAlsoExists(string colorName)
        {
            var color = GetByName(colorName);

            return color != null;
        }

        public IEnumerable<DisplayColorDto> GetAll()
        {
            return _unitOfWork.Color.GetAll().Select(x => new DisplayColorDto { Id  = x.Id, Name = x.Name});
        }

        public DisplayColorDto? GetById(int id)
        {
            var currentColor = _unitOfWork.Color.FindOne(x => x.Id == id);

            if(currentColor ==null) {
                return null;
            }

            var colorDisplay = new DisplayColorDto { Id = currentColor.Id, Name = currentColor.Name };
            return colorDisplay;

        }

        public DisplayColorDto? GetByName(string colorName)
        {
            var currentColor = _unitOfWork.Color.FindOne(x => x.Name == colorName);

            if (currentColor == null)
            {
                return null;
            }

            var colorDisplay = new DisplayColorDto { Id = currentColor.Id, Name = currentColor.Name};
            return colorDisplay;
        }
    }
}
