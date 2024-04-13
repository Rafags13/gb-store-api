using GbStoreApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GbStoreApi.Data.Extensions
{
    public static class ProductExtensions
    {
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
                .AsEnumerable()
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
    }
}
