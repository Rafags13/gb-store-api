using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Purchases;
using GbStoreApi.Domain.Enums;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GbStoreApi.Application.Services.Purchases
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAddressService _addressService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public PurchaseService(
            IUnitOfWork unitOfWork,
            IAddressService addressService,
            IUserService userService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _addressService = addressService;
            _userService = userService;
            _mapper = mapper;
        }
        public ResponseDto<bool> BuyProduct(BuyProductDto buyProductDto)
        {
            if (SomeStockIsUnavaliable(buyProductDto.Items))
                return new ResponseDto<bool>(StatusCodes.Status409Conflict, "O estoque selecionado atual possui itens inválidos.");

            var response =
                buyProductDto.TypeOfDelivery == DeliveryType.StorePickup ?
                _addressService.GetAddressIdFromStorePickup() :
                _addressService.GetAddressIdByZipCode(buyProductDto.ZipCode);

            if (response.StatusCode != StatusCodes.Status200OK)
                return new ResponseDto<bool>(response.StatusCode, response.Message);

            var newPurchase = _mapper.Map<Purchase>(buyProductDto);
            newPurchase.DeliveryAddressId = response.Value;

            _unitOfWork.Purchase.Add(newPurchase);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status422UnprocessableEntity, "Não foi possível realizar a compra.");

            return new ResponseDto<bool>(StatusCodes.Status200OK);
                
        }

        public ResponseDto<IEnumerable<PurchaseSpecificationDto>> GetAll()
        {
            var currentUser = _userService.GetCurrentInformations();

            if (currentUser.StatusCode != StatusCodes.Status200OK || currentUser.Value is null)
                return new ResponseDto<IEnumerable<PurchaseSpecificationDto>>(currentUser.StatusCode, currentUser.Message!);

            var currentBoughts =
                _unitOfWork.Purchase
                .GetAll()
                .Include(x => x.DeliveryAddress)
                    .ThenInclude(x => x.UserOwner)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Stock)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Pictures)
                .Include(x => x.OrderItems)
                .Select(_mapper.Map<PurchaseSpecificationDto>)
                .Where(x => x.BoughterId == currentUser.Value.Id);

            return new ResponseDto<IEnumerable<PurchaseSpecificationDto>>(currentBoughts, StatusCodes.Status200OK);
        }

        private bool SomeStockIsUnavaliable(IEnumerable<CreateOrderItemDto> items)
        {
            var itemsIds = items.Select(x => x.ProductStockId);
            var allStockFromDatabase =
                _unitOfWork.Stock
                    .GetAll()
                    .Where(x => itemsIds.Contains(x.Id))
                    .AsNoTracking();

            var unavaliable = false;

            items.ForEach(item =>
            {
                var currentItemFromDatabase = allStockFromDatabase.FirstOrDefault(predicate: x => x.Id == item.ProductStockId);

                if(item.ProductCount > currentItemFromDatabase.Id)
                {
                    unavaliable = true;
                }
            });

            return unavaliable;
        }
    }
}
