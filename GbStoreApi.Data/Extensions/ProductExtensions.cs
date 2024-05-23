using GbStoreApi.Domain.Enums;
using GbStoreApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace GbStoreApi.Data.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> WithPicturesFromStock(this IQueryable<Product> products)
        {
            return products
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Pictures);
        }
        public static IQueryable<Product> WithColors(this IQueryable<Product> products)
        {
            return products.Include(x => x.Stocks)
                .ThenInclude(x => x.Color);
        }
        public static IQueryable<Product> WithSizes(this IQueryable<Product> products)
        {
            return products.Include(x => x.Stocks)
                .ThenInclude(x => x.Size);
        }
        public static IQueryable<Product> FilterByCategoryIfWasInformed(this IQueryable<Product> products, string? categoryName)
        {
            return products.Include(x => x.Category).Where(x => string.IsNullOrEmpty(categoryName) || x.Category!.Name == categoryName);
        }

        public static IQueryable<Product> FilterByColorsIfWereInformed(this IQueryable<Product> products, string[]? colors)
        {
            return products
                .SelectMany(x => x.Stocks)
                .GroupBy(x => x.ProductId)
                .AsEnumerable()
                .Where(stock => 
                    !colors.Any() ||
                     colors.All(colorName => stock
                        .Select(color => color.Color.Name)
                        .Distinct()
                        .Contains(colorName)
                ))
                .SelectMany(x => x.Select(x => x.Product!).DistinctBy(x => x.Id))
                .AsQueryable();
        }

        public static IQueryable<Product> FilterBySizesIfWereInformed(this IQueryable<Product> products, string[]? sizes)
        {
            return products
                .SelectMany(x => x.Stocks)
                .GroupBy(x => x.ProductId)
                .Where(stock =>
                    !sizes.Any() ||
                     sizes.All(sizeName => stock
                        .Select(size => size.Size.Name)
                        .Distinct()
                        .Contains(sizeName)
                ))
                .SelectMany(x => x.Select(x => x.Product!).DistinctBy(x => x.Id))
                .AsQueryable();
        }
        
        public static IQueryable<Product> Paginate(this IQueryable<Product> products, int page = 0, int pageSize = 20)
        {
            return products.Skip(page * pageSize).Take(pageSize);
        }

        public static IQueryable<Product> OrderBy(this IQueryable<Product> products, string direction, string fieldName)
        {
            return products.OrderBy($"{fieldName} {direction}");
        }
    }
}
