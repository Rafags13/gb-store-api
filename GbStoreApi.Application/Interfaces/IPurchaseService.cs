using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Purchases;

namespace GbStoreApi.Application.Interfaces
{
    public interface IPurchaseService
    {
        ResponseDto<bool> BuyProduct(BuyProductDto buyProductDto);
        ResponseDto<IEnumerable<PurchaseSpecificationDto>> GetAll();
    }
}
