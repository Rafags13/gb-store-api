using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Enums;
using GbStoreApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core;

namespace GbStoreApi.Data.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> WithPictures(this IQueryable<Product> products)
        {
            return products.Include(x => x.Pictures);
        }

        public static IQueryable<Product> WithColors(this IQueryable<Product> products)
        {
            return products
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Color);
        }

        public static IQueryable<Product> WithSizes(this IQueryable<Product> products)
        {
            return products
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Size);
        }

        public static IQueryable<Product> WithCategories(this IQueryable<Product> products)
        {
            return products.Include(x => x.Category);
        }

        public static IEnumerable<DisplayProductDto> FilterByCategoryIfWasInformed(this IEnumerable<DisplayProductDto> products, string? categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return products;

            return products
                .Where(x => x.Category == categoryName);
        }

        public static IEnumerable<DisplayProductDto> FilterByColorsIfWereInformed(this IEnumerable<DisplayProductDto> products, string[]? colors)
        {
            if (colors is null || !colors.Any())
                return products;

            return products.Where(product => colors.All(colorName => product.Colors.Contains(colorName)));
        }

        public static IEnumerable<DisplayProductDto> FilterBySizesIfWereInformed(this IEnumerable<DisplayProductDto> products, string[]? sizes)
        {
            if(sizes is null || !sizes.Any())
                return products;

            return products.Where(product => sizes.All(sizeName => product.Sizes.Contains(sizeName)));
        }
        
        public static IEnumerable<DisplayProductDto> Paginate(this IEnumerable<DisplayProductDto> products, int page = 0, int pageSize = 20)
        {
            return products.Skip(page * pageSize).Take(pageSize);
        }

        public static IEnumerable<DisplayProductDto> OrderBy(this IEnumerable<DisplayProductDto> products, string? direction, string? fieldName)
        {
            if (string.IsNullOrWhiteSpace(direction) || string.IsNullOrWhiteSpace(fieldName))
                return products.OrderBy(x => x.Id);

            return products.OrderBy(direction, fieldName);
        }
    }
}
