using GbStoreApi.Application.Interfaces;
using GbStoreApi.Application.Services.AmazonBuckets;
using GbStoreApi.Application.Services.Authentication;
using GbStoreApi.Application.Services.Authentications;
using GbStoreApi.Application.Services.Brands;
using GbStoreApi.Application.Services.Categories;
using GbStoreApi.Application.Services.Colors;
using GbStoreApi.Application.Services.Pictures;
using GbStoreApi.Application.Services.Products;
using GbStoreApi.Application.Services.Sizes;
using GbStoreApi.Application.Services.Stock;
using GbStoreApi.Application.Services.Users;
using GbStoreApi.Data.Implementation;
using GbStoreApi.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace GbStoreApi.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddProductServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBrandService, BrandService>();
        serviceCollection.AddScoped<ISizeService, SizeService>();
        serviceCollection.AddScoped<IColorService, ColorService>();
        serviceCollection.AddScoped<IStockService, StockService>();
        serviceCollection.AddScoped<IPictureService, PictureService>();
        serviceCollection.AddScoped<IProductService, ProductService>();

        serviceCollection.AddScoped<ICategoryService, CategoryService>();

        return serviceCollection;
    }

    public static IServiceCollection AddUserServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<ITokenService, TokenService>();

        return serviceCollection;
    }

    public static IServiceCollection AddDifferentUnitOfWork(this IServiceCollection servicesCollection)
    {
        servicesCollection.AddTransient<IUnitOfWork, UnitOfWork>();
        return servicesCollection;
    }

    public static IServiceCollection AddFileServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBucketService, BucketService>();
        serviceCollection.AddScoped<IFileService, FileService>();

        return serviceCollection;
    }

}

