using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Enums;
using GbStoreApi.Domain.Models;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public static IQueryable<DisplayProductDto> FilterByCategoryIfWasInformed(this IQueryable<DisplayProductDto> products, string? categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return products;

            return products
                .Where(x => x.Category == categoryName);
        }

        public static IQueryable<DisplayProductDto> FilterByColorsIfWereInformed(this IQueryable<DisplayProductDto> products, string[]? colors)
        {
            if (colors is null || !colors.Any())
                return products;

            return products.Where(product => product.Colors.Any(x => colors.Contains(x)));
        }

        public static IQueryable<DisplayProductDto> FilterBySizesIfWereInformed(this IQueryable<DisplayProductDto> products, string[]? sizes)
        {
            if(sizes is null || !sizes.Any())
                return products;

            return products.Where(product => product.Sizes.Any(x => sizes.Contains(x)));
        }
        
        public static IQueryable<DisplayProductDto> Paginate(this IQueryable<DisplayProductDto> products, int page = 0, int pageSize = 20)
        {
            return products.Skip(page * pageSize).Take(pageSize);
        }

        public static IQueryable<DisplayProductDto> OrderBy(this IQueryable<DisplayProductDto> products, string? direction, string? fieldName)
        {
            if (string.IsNullOrWhiteSpace(direction) || string.IsNullOrWhiteSpace(fieldName))
                return products.OrderBy(x => x.Id);
            
            return products.AsQueryable().OrderBy($"{fieldName} {direction}");
        }

        public static IQueryable<DisplayProductDto> Count(this IQueryable<DisplayProductDto> products, out int count)
        {
            count = products.Count();

            return products;
        }
    }
}
