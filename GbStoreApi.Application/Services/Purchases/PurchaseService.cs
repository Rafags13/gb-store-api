using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Purchases;
using GbStoreApi.Domain.Enums;
using GbStoreApi.Domain.Models.Purchases;
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
                return new ResponseDto<bool>(
                    StatusCodes.Status409Conflict,
                    "Algum dos produtos está com estoque indisponível. " +
                    "Reinicie a página para visualizar no carrinho esse item.");

            var loggedUserId = _userService.GetLoggedUserId();

            using var transaction = _unitOfWork.GetContext().BeginTransaction();

            if (buyProductDto.TypeOfDelivery.Equals(DeliveryType.StorePickup))
            {
                var storeAddressId = _unitOfWork.Address.GetStoreAddressId();

                if (storeAddressId is null)
                    return new ResponseDto<bool>(
                        StatusCodes.Status400BadRequest,
                        "Não foi possível buscar o endereço da loja. Tente Novamente.");

                TryAddStorePickupPurchase(buyProductDto, loggedUserId, storeAddressId.GetValueOrDefault());
            }
            else
            {
                var userAddressId = _unitOfWork.UserAddresses.GetUserAddressIdByUserAndZipCode(
                    zipCode: buyProductDto.SelectedZipCode ?? "",
                    loggedUserId
                    );

                if (userAddressId is null)
                    return new ResponseDto<bool>(
                        StatusCodes.Status400BadRequest,
                        "Não foi possível encontrar o endereço escolhido para entrega. Tente Novamente.");

                TryAddShippingPurchase(buyProductDto, userAddressId.GetValueOrDefault());
            }

            UpdateStockItemsAfterBought(buyProductDto.Items);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status422UnprocessableEntity, "Não foi possível realizar a compra.");

            transaction.Commit();

            return new ResponseDto<bool>(StatusCodes.Status200OK);
        }

        private bool SomeStockIsUnavaliable(IEnumerable<CreateOrderItemDto> items)
        {
            var itemsIds = items.Select(x => x.ProductStockId);
            var hasLeastOneStockIsUnavaliable =
                _unitOfWork.Stock
                    .GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Count
                    })
                    .Where(x => itemsIds.Contains(x.Id))
                    .AsNoTracking()
                    .ToList()
                    .Zip(items)
                    .Any((items) => 
                        items.First.Id == items.Second.ProductStockId &&
                        items.First.Count < items.Second.ProductCount);

            return hasLeastOneStockIsUnavaliable;
        }

        private void TryAddStorePickupPurchase(BuyProductDto buyProductDto, int loggedUserId, int storeAddressId)
        {
            var storePickupPurchase = _mapper.Map<StorePickupPurchase>(buyProductDto, opt => opt.AfterMap((_, purchase) =>
            {
                purchase.StoreAddressId = storeAddressId;
                purchase.UserBuyerId = loggedUserId;
            }));

            _unitOfWork.StorePickupPurchase.Add(storePickupPurchase);
        }

        private void TryAddShippingPurchase(BuyProductDto buyProductDto, Guid userAddressId)
        {
            var shippingPurchase = _mapper.Map<ShippingPurchase>(buyProductDto, opt => opt.AfterMap((_, purchase) =>
            {
                purchase.UserAddressId = userAddressId;
            }));

            _unitOfWork.ShippingPurchase.Add(shippingPurchase);
        }

        private void UpdateStockItemsAfterBought(IEnumerable<CreateOrderItemDto> items)
        {
            var itemsIds = items.Select(x => x.ProductStockId);

            var currentStockProducts =
                _unitOfWork.Stock.GetAll()
                .Where(x => itemsIds.Contains(x.Id));

            Parallel.ForEach(currentStockProducts, x =>
            {
                var currentStock = items.First(itemStock => itemStock.ProductStockId == x.Id);

                x.Count -= currentStock.ProductCount;
            });

            _unitOfWork.Stock.UpdateRange(currentStockProducts);
        }

        public ResponseDto<IEnumerable<PurchaseSpecificationDto>> GetAll()
        {
            var currentUser = _userService.GetLoggedUserId();

            var currentBoughts =
                _unitOfWork.Purchase
                .GetAll()
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Stock)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Pictures)
                .Include(x => x.ShippingPurchase)
                    .ThenInclude(x => x.UserOwnerAddress)
                        .ThenInclude(x => x.Address)
                .Include(x => x.StorePickupPurchase)
                    .ThenInclude(x => x.StoreAddress)
                .AsNoTracking()
                .Select(_mapper.Map<PurchaseSpecificationDto>)
                .Where(x => x.BoughterId == currentUser);

            return new ResponseDto<IEnumerable<PurchaseSpecificationDto>>(currentBoughts, StatusCodes.Status200OK);
        }
    }
}
