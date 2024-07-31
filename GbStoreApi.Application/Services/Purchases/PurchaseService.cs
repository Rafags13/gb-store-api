using AutoMapper;
using AutoMapper.QueryableExtensions;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Data.Extensions;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Purchases;
using GbStoreApi.Domain.Enums;
using GbStoreApi.Domain.Models.Purchases;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GbStoreApi.Application.Services.Purchases
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public PurchaseService(
            IUnitOfWork unitOfWork,
            IUserService userService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
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

            return new ResponseDto<IEnumerable<PurchaseSpecificationDto>>(currentBoughts);
        }

        public PaginatedResponseDto<IEnumerable<AdminPurchaseDisplay>> GetPaginated(
            string searchQuery = "",
            int page = 0,
            int pageSize = 20
            )
        {
            var paginatedProducts = _unitOfWork.Purchase
                .GetAll()
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Stock)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Pictures)
                .Include(x => x.ShippingPurchase)
                    .ThenInclude(x => x.UserOwnerAddress)
                        .ThenInclude(x => x.User)
                .Include(x => x.StorePickupPurchase)
                    .ThenInclude(x => x.UserBuyer)
                .ProjectTo<AdminPurchaseDisplay>(_mapper.ConfigurationProvider)
                .FilterByBoughterName(searchQuery)
                .GetCount(out int totalItems)
                .Paginate(page, pageSize);

            return new(paginatedProducts, page, pageSize, totalItems);
        }

        public ResponseDto<AdminPurchaseSpecificationDto> GetSpecificationById(int id)
        {
            var purchase = _unitOfWork.Purchase
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
                .Select(_mapper.Map<AdminPurchaseSpecificationDto>)
                .FirstOrDefault(x => x.PurchaseId == id);

            if (purchase is null)
                return new(StatusCodes.Status404NotFound, "A compra buscada não existe mais.");

            return new(purchase);
        }

        public ResponseDto<bool> UpdateStateById(int id, PurchaseState newState)
        {
            var purchase = _unitOfWork.Purchase.FindOne(x => x.Id == id);

            if (purchase is null)
                return new (StatusCodes.Status404NotFound, "Essa compra não existe.");

            if (purchase.PurchaseState.Equals(PurchaseState.Delivered))
                return new(StatusCodes.Status400BadRequest, "Essa compra já foi entregue.");

            if (purchase.PurchaseState.Equals(newState))
                return new(StatusCodes.Status400BadRequest, "A compra já foi atualizada para esse estado.");

            purchase.PurchaseState = newState;

            _unitOfWork.Purchase.Update(purchase);

            if (_unitOfWork.Save() == 0)
                return new(StatusCodes.Status422UnprocessableEntity, "Não foi possível alterar o estado da Compra. Tente Novamente.");

            return new(true);
        }

        public (int, int) GetDifferenceCountByMonthIndex(int monthIndex)
        {
            var sellsCountingByMonthIndex = _unitOfWork.Purchase.CountByMonthIndex(monthIndex);
            var previousSellCoutingByMonthIndex = _unitOfWork.Purchase.CountByMonthIndex(monthIndex == 1 ? 12 : monthIndex - 1);

            return new (sellsCountingByMonthIndex, previousSellCoutingByMonthIndex);
        }

        public (decimal, decimal) GetDifferenceSumByMonthIndex(int monthIndex)
        {
            var sellsSummingByMonthIndex = _unitOfWork.Purchase.SumByMonthIndex(monthIndex);
            var previousSellSummingByMonthIndex = _unitOfWork.Purchase.SumByMonthIndex(monthIndex == 1 ? 12 : monthIndex - 1);

            return new(sellsSummingByMonthIndex, previousSellSummingByMonthIndex);
        }

        public (int, int) GetMultipleProductsDifferenceCountByMonthIndex(int monthIndex)
        {
            var sellsWithMultipleProductsByMonthIndex = _unitOfWork.Purchase
                .GetAll()
                .Include(x => x.OrderItems)
                .Where(x => x.OrderDate.Month == monthIndex)
                .Count(x => x.OrderItems.Count > 1);

            var previousSellWithMultipleProducts = _unitOfWork.Purchase
                .GetAll()
                .Include(x => x.OrderItems)
                .Where(x => x.OrderDate.Month == (monthIndex == 1 ? 12 : monthIndex - 1))
                .Count(x => x.OrderItems.Count > 1);

            return new(sellsWithMultipleProductsByMonthIndex, previousSellWithMultipleProducts);
        }
    }
}
