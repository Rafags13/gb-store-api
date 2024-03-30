namespace GbStoreApi.Domain.Dto
{
    public class DisplayVariantsDto
    {
        public DisplayVariantsDto() { }

        public IEnumerable<DisplaySizeDto> Sizes { get; set; } = Enumerable.Empty<DisplaySizeDto>();
        public IEnumerable<DisplayColorDto> Colors { get; set; } = Enumerable.Empty<DisplayColorDto>();
    }
}
