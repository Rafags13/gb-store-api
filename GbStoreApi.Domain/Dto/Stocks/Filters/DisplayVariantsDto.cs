using GbStoreApi.Domain.Dto.Colors;
using GbStoreApi.Domain.Dto.Sizes;

namespace GbStoreApi.Domain.Dto.Stocks.Filters
{
    public class DisplayVariantsDto
    {
        public DisplayVariantsDto() { }

        public IEnumerable<DisplaySizeDto> Sizes { get; set; } = Enumerable.Empty<DisplaySizeDto>();
        public IEnumerable<DisplayColorDto> Colors { get; set; } = Enumerable.Empty<DisplayColorDto>();
    }
}
