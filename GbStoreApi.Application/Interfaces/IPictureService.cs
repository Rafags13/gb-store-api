using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface IPictureService
    {
        Task<string> GetPictures();
        Task<string> GetPictureById();
        void CreatePicture(CreatePictureDto createPicture);
        void CreateMultiplePictures(CreateMultiplePicturesDto picturesToCreate);
    }
}
