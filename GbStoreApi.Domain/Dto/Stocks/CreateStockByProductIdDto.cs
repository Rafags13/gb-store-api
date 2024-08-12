namespace GbStoreApi.Domain.Dto.Stocks
{
    public class CreateStockByProductIdDto
    {
        public CreateStockByProductIdDto() { }
        public int ProductId { get; set; }
        public IEnumerable<CreateStockDto> Variants { get; set; } = Enumerable.Empty<CreateStockDto>();
    }
}
