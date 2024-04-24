namespace GbStoreApi.Domain.Dto.Pictures
{
    public class PicturesGroupedByProductDto
    {
        public required IEnumerable<string> Pictures { get; set; }
        public int ProductId { get; set; }
    }
}
