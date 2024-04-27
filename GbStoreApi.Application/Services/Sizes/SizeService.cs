using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Sizes;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GbStoreApi.Application.Services.Sizes
{
    public class SizeService : ISizeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SizeService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region [CRUD]
        public ResponseDto<DisplaySizeDto> Create(string sizeName)
        {
            var newSize = new Size { Name = sizeName };
            _unitOfWork.Size.Add(newSize);
            _unitOfWork.Save();

            var response = GetByName(sizeName);
            if (response.StatusCode != StatusCodes.Status200OK || response.Value is null)
                return new ResponseDto<DisplaySizeDto>(response.StatusCode, response.Message!);

            return new ResponseDto<DisplaySizeDto>(response.Value, response.StatusCode);
        }

        public ResponseDto<DisplaySizeDto> Delete(int id)
        {
            var currentSize =
                _unitOfWork
                .Size
                .GetByIdAndReturnsQueryable(id)
                .Include(x => x.Stocks)
                .FirstOrDefault();

            if (currentSize is null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não existe nenhum tamanho com esse Id.");

            if (currentSize.Stocks is not null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status400BadRequest,
                    "Não é possível excluir esse tamanho, pois este está ligado a um produto. Exclua esta relação para por excluí-lo");

            var removedSize = _unitOfWork.Size.Remove(currentSize);
            _unitOfWork.Save();

            var sizeToResponse = _mapper.Map<DisplaySizeDto>(removedSize);

            return new ResponseDto<DisplaySizeDto>(sizeToResponse, StatusCodes.Status200OK);
        }

        public ResponseDto<IEnumerable<DisplaySizeDto>> GetAll()
        {
            var allSizes = _unitOfWork.Size.GetAll().Select(size => _mapper.Map<DisplaySizeDto>(size)) ??
                Enumerable.Empty<DisplaySizeDto>().AsQueryable();

            return new ResponseDto<IEnumerable<DisplaySizeDto>>(allSizes, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplaySizeDto> GetById(int id)
        {
            var currentSize = _unitOfWork.Size.GetById(id);
            if (currentSize == null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não existe nenhum tamanho com esse Id.");

            var size = _mapper.Map<DisplaySizeDto>(currentSize);

            return new ResponseDto<DisplaySizeDto>(size, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplaySizeDto> GetByName(string sizeName)
        {
            var currentSize = _unitOfWork.Size.FindOne(x => x.Name == sizeName);
            if (currentSize == null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não existe nenhum tamanho com esse Id.");

            var size = _mapper.Map<DisplaySizeDto>(currentSize);

            return new ResponseDto<DisplaySizeDto>(size, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplaySizeDto> Update(UpdateSizeDto updateSizeDto)
        {
            var currentSize = _unitOfWork.Size.GetByName(updateSizeDto.OldSizeName);

            if (currentSize is null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não existe nenhum tamanho com esse nome.");

            currentSize.Name = updateSizeDto.NewSizeName;

            var updatedSize = _unitOfWork.Size.Update(currentSize);
            _unitOfWork.Save();
            var sizeToResponse = _mapper.Map<DisplaySizeDto>(updatedSize);

            return new ResponseDto<DisplaySizeDto>(sizeToResponse, StatusCodes.Status200OK);
        }
        #endregion
    }
}
