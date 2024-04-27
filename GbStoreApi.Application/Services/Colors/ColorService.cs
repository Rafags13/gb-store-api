﻿using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Colors;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GbStoreApi.Application.Services.Colors
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ColorService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region [CRUD]
        public ResponseDto<DisplayColorDto> CreateColor(string colorName)
        {
            if (_unitOfWork.Color.Contains(color => color.Name == colorName))
                return new ResponseDto<DisplayColorDto>(StatusCodes.Status400BadRequest, "A cor informada já existe no sistema.");
            
            var newColor = new Color { Name = colorName };
            _unitOfWork.Color.Add(newColor);
            _unitOfWork.Save();

            var currentColor = _unitOfWork.Color.GetByName(colorName);
            var createdColor = _mapper.Map<DisplayColorDto>(currentColor);

            return new ResponseDto<DisplayColorDto>(createdColor, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayColorDto> Delete(int id)
        {
            var currentColor = 
                _unitOfWork
                .Color
                .GetByIdAndReturnsQueryable(id)
                .Include(x => x.Stocks)
                .FirstOrDefault();

            if (currentColor is null)
                return new ResponseDto<DisplayColorDto>(StatusCodes.Status404NotFound, "Não existe nenhuma cor com esse nome.");

            if(currentColor.Stocks is not null)
                return new ResponseDto<DisplayColorDto>(StatusCodes.Status400BadRequest,
                    "A cor selecionada está relacionada a outros produtos. Delete a relação para remover esta cor.");

            var removedColor = _unitOfWork.Color.Remove(currentColor);
            _unitOfWork.Save();

            var colorToResponse = _mapper.Map<DisplayColorDto>(removedColor);

            return new ResponseDto<DisplayColorDto>(colorToResponse, StatusCodes.Status200OK);
        }

        public ResponseDto<IEnumerable<DisplayColorDto>> GetAll()
        {
            var colors = _unitOfWork.Color.GetAll().Select(color => _mapper.Map<DisplayColorDto>(color));

            return new ResponseDto<IEnumerable<DisplayColorDto>>(colors, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayColorDto> GetById(int id)
        {
            var currentColor = _unitOfWork.Color.FindOne(x => x.Id == id);

            if (currentColor is null)
                return new ResponseDto<DisplayColorDto>(StatusCodes.Status404NotFound, "Não existe nenhuma cor com o Id informado.");

            var color = _mapper.Map<DisplayColorDto>(currentColor);

            return new ResponseDto<DisplayColorDto>(color, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayColorDto> GetByName(string colorName)
        {
            var currentColor = _unitOfWork.Color.FindOne(x => x.Name == colorName);

            if (currentColor == null)
                return new ResponseDto<DisplayColorDto>(StatusCodes.Status404NotFound, "Não existe nenhuma cor com o Id informado.");

            var color = _mapper.Map<DisplayColorDto>(currentColor);

            return new ResponseDto<DisplayColorDto>(color, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayColorDto> Update(UpdateColorDto updateColorDto)
        {
            var currentColor = _unitOfWork.Color.GetByName(updateColorDto.OldColorName);

            if (currentColor is null)
                return new ResponseDto<DisplayColorDto>(StatusCodes.Status404NotFound, "Não existe nenhuma cor com esse nome.");

            currentColor.Name = updateColorDto.NewColorName;
            var updatedColor = _unitOfWork.Color.Update(currentColor);
            _unitOfWork.Save();
            
            var colorToResponse = _mapper.Map<DisplayColorDto>(updatedColor);

            return new ResponseDto<DisplayColorDto>(colorToResponse, StatusCodes.Status200OK);

        }
        #endregion
    }
}
