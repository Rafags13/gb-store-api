using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Purchases;
using GbStoreApi.Domain.Enums;

namespace GbStoreApi.Application.Interfaces
{
    public interface IPurchaseService
    {
        ResponseDto<bool> BuyProduct(BuyProductDto buyProductDto);
        ResponseDto<IEnumerable<PurchaseSpecificationDto>> GetAll();
        PaginatedResponseDto<IEnumerable<AdminPurchaseDisplay>> GetPaginated(
            string searchQuery = "",
            int page = 0,
            int pageSize = 20
            );
        ResponseDto<AdminPurchaseSpecificationDto> GetSpecificationById(int id);
        ResponseDto<bool> UpdateStateById(int id, PurchaseState newState);
    }
}
