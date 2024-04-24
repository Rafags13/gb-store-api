using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Pictures;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Application.Services.Pictures
{
    public class PictureService : IPictureService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PictureService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateMultiplePictures(PicturesGroupedByProductDto picturesToCreate)
        {
            var newPictures = picturesToCreate.Pictures.Select(pictureName => new Picture
            {
                Name = pictureName,
                ProductId = picturesToCreate.ProductId
            });

            _unitOfWork.Picture.AddRange(newPictures);
            _unitOfWork.Save();
        }

        public void CreatePicture(string createPicture)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPictureById()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPictures()
        {
            throw new NotImplementedException();
        }
    }
}
