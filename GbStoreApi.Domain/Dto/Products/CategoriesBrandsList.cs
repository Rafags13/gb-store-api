using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Categories;

namespace GbStoreApi.Domain.Dto.Products
{
    public class CategoriesBrandsList
    {
        public IEnumerable<DisplayCategoryDto> Categories { get; set; }
        public IEnumerable<DisplayBrandDto> Brands { get; set; }

        public CategoriesBrandsList() {}

        public CategoriesBrandsList(IEnumerable<DisplayCategoryDto> categories, IEnumerable<DisplayBrandDto> brands)
        {
            Categories = categories;
            Brands = brands;
        }
    }
}
