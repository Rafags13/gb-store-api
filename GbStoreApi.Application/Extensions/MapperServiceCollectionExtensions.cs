﻿using AutoMapper;
using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Stocks;
using GbStoreApi.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GbStoreApi.Application.Extensions
{
    public static class MapperServiceCollectionExtensions
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            var config = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<Product, DisplayProductDto>()
                    .ForMember(member => member.RealPrice, map => map.MapFrom(x => x.UnitaryPrice))
                    .ForMember(member => member.PhotoUrlId, map => map.MapFrom(x => x.Pictures.FirstOrDefault().Name))
                    .ForMember(member => member.VariantNames, map => map.MapFrom(x => x.Stocks.Select(stocks => stocks.Color!.Name)
                                                 .Concat(x.Stocks.Select(color => color.Size!.Name)).Distinct()))
                    .ReverseMap();

                configuration.CreateMap<Product, ProductSpecificationsDto>()
                    .ForMember(member => member.RealPrice, map => map.MapFrom(x => x.UnitaryPrice))
                    .ForMember(member => member.ProductPictureIds, map => map.MapFrom(x => x.Pictures.Select(y => y.Name)))
                    .ForMember(member => member.Category, map => map.MapFrom(x => x.Category!.Name))
                    .ReverseMap();
                configuration.CreateMap<ProductStock, StockDto>()
                    .ForMember(member => member.StockId, map => map.MapFrom(x => x.Id))
                    .ForMember(member => member.Amount, map => map.MapFrom(x => x.Count))
                    .ReverseMap();
            });

            IMapper mapper = config.CreateMapper();

            return services.AddSingleton(mapper);
        }
    }
}
