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
        public ResponseDto<IEnumerable<DisplaySizeDto>> GetAll()
        {
            var allSizes = _unitOfWork.Size.GetAll().Select(size => _mapper.Map<DisplaySizeDto>(size)) ??
                Enumerable.Empty<DisplaySizeDto>().AsQueryable();

            return new ResponseDto<IEnumerable<DisplaySizeDto>>(allSizes);
        }

        public ResponseDto<DisplaySizeDto> GetById(int id)
        {
            var currentSize = _unitOfWork.Size.GetById(id);
            if (currentSize == null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não existe nenhum tamanho com esse Id.");

            var size = _mapper.Map<DisplaySizeDto>(currentSize);

            return new ResponseDto<DisplaySizeDto>(size);
        }

        public ResponseDto<DisplaySizeDto> GetByName(string sizeName)
        {
            var currentSize = _unitOfWork.Size.FindOne(x => x.Name == sizeName);
            if (currentSize == null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não existe nenhum tamanho com esse Id.");

            var size = _mapper.Map<DisplaySizeDto>(currentSize);

            return new ResponseDto<DisplaySizeDto>(size);
        }

        public ResponseDto<DisplaySizeDto> Create(string sizeName)
        {
            var newSize = new Size { Name = sizeName };

            _unitOfWork.Size.Add(newSize);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status422UnprocessableEntity, "Não foi possível adicionar o tamanho.");
            
            var recentllyAddedSize = _unitOfWork.Size.GetByName(sizeName);

            if (recentllyAddedSize is null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não foi possível buscar o recém adicionado tamanho.");

            var response = _mapper.Map<DisplaySizeDto>(recentllyAddedSize);

            return new ResponseDto<DisplaySizeDto>(response);
        }

        public ResponseDto<DisplaySizeDto> Update(UpdateSizeDto updateSizeDto)
        {
            var currentSize = _unitOfWork.Size.GetByName(updateSizeDto.OldSizeName);

            if (currentSize is null)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status404NotFound, "Não existe nenhum tamanho com esse nome.");

            currentSize.Name = updateSizeDto.NewSizeName;

            var updatedSize = _unitOfWork.Size.Update(currentSize);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status400BadRequest, "Não foi possível atualizar o tamanho.");
            
            var sizeToResponse = _mapper.Map<DisplaySizeDto>(updatedSize);

            return new ResponseDto<DisplaySizeDto>(sizeToResponse);
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

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<DisplaySizeDto>(StatusCodes.Status422UnprocessableEntity, "Não foi possível excluir o tamanho.");

            var sizeToResponse = _mapper.Map<DisplaySizeDto>(removedSize);

            return new ResponseDto<DisplaySizeDto>(sizeToResponse);
        }
        #endregion
    }
}
