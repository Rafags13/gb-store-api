﻿using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int CreateMultipleStock(CreateStockWithIdDto createStockDto)
        {
            var stocks = 
                createStockDto
                .Variants
                .Select(x => new ProductStock { 
                    ProductId = createStockDto.ProductId,
                    ColorId = x.ColorId,
                    SizeId = x.SizeId,
                    Count = x.Count
                });

            _unitOfWork.Stock.AddRange(stocks);

            return _unitOfWork.Save();
        }
    }
}
