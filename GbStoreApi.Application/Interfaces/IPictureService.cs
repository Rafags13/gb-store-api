using GbStoreApi.Domain.Dto.Pictures;

namespace GbStoreApi.Application.Interfaces
{
    public interface IPictureService
    {
        Task<string> GetPictures();
        Task<string> GetPictureById();
        void CreatePicture(string createPicture);
        void CreateMultiplePictures(PicturesGroupedByProductDto picturesToCreate);
    }
}
