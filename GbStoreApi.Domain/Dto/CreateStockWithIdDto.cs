namespace GbStoreApi.Domain.Dto
{
    public class CreateStockWithIdDto
    {
        public CreateStockWithIdDto(){}
        public int ProductId { get; set; }
        public IEnumerable<CreateStockDto> Variants { get; set; } = Enumerable.Empty<CreateStockDto>();
    }
}
