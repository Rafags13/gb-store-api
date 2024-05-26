﻿using AutoMapper;
using GbStoreApi.Domain.Dto.Address;
using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Categories;
using GbStoreApi.Domain.Dto.Colors;
using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Purchases;
using GbStoreApi.Domain.Dto.Sizes;
using GbStoreApi.Domain.Dto.Stocks;
using GbStoreApi.Domain.Dto.UserAddresses;
using GbStoreApi.Domain.Dto.Users;
using GbStoreApi.Domain.enums;
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
                #region [Product]
                configuration.CreateMap<Product, DisplayProductDto>()
                    .ForMember(member => member.RealPrice, map => map.MapFrom(x => x.UnitaryPrice))
                    .ForMember(member => member.PhotoUrlId, map => map.MapFrom(x => x.Pictures.FirstOrDefault().Name))
                    .ForMember(member => member.Colors, map => map.MapFrom(src => src.Stocks.Select(stocks => stocks.Color!.Name).Distinct()))
                    .ForMember(member => member.Sizes, map => map.MapFrom(src => src.Stocks.Select(stocks => stocks.Color!.Name).Distinct()))
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
                #endregion

                #region [Brand]
                configuration.CreateMap<Brand, DisplayBrandDto>();
                #endregion

                #region [User]
                configuration.CreateMap<DisplayUserDto, User>();
                configuration.CreateMap<User, DisplayUserDto>();
                configuration.CreateMap<UserTokenDto, User>().ReverseMap();
                configuration.CreateMap<SignUpDto, User>()
                    .ForMember(member => member.Password, map => map.MapFrom(x => BCrypt.Net.BCrypt.HashPassword(x.Password)))
                    .ForMember(member => member.TypeOfUser, map => map.MapFrom(x => (int) x.TypeOfUser));
                configuration.CreateMap<RefreshToken, User>()
                    .ForMember(member => member.RefreshToken, map => map.MapFrom(x => x.Token));

                configuration.CreateMap<UpdateUserDto, User>();
                #endregion

                #region [Size]
                configuration.CreateMap<DisplaySizeDto, Size>().ReverseMap();
                #endregion

                #region [Category]
                configuration.CreateMap<DisplayCategoryDto, Category>().ReverseMap();
                #endregion

                #region [Color]
                configuration.CreateMap<DisplayColorDto, Color>().ReverseMap();
                #endregion

                #region [Address]
                configuration.CreateMap<BaseAddressDto, Address>().ReverseMap();

                configuration.CreateMap<Address, DisplayAddressDto>()
                    .IncludeBase<Address, BaseAddressDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.UserId, opt => opt.Ignore())
                    .ReverseMap();

                configuration.CreateMap<CreateAddressDto, Address>()
                    .IncludeBase<DisplayAddressDto, Address>();

                configuration.CreateMap<UpdateAddressDto, Address>();
                #endregion

                #region UserAddress
                configuration.CreateMap<CreateUserAddressByAddress, UserAddress>();

                configuration.CreateMap<UserAddress, DisplayAddressDto>()
                    .IncludeMembers(src => src.Address)
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId));
                #endregion

                #region [Product]
                configuration.CreateMap<CreateProductDto, Product>();
                #endregion

                #region [Purchase]
                configuration.CreateMap<BuyProductDto, Purchase>()
                    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                    .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.Items.Sum(x => x.ProductStockPrice)));
                configuration.CreateMap<CreateOrderItemDto, OrderItems>();
                configuration.CreateMap<Purchase, PurchaseSpecificationDto>()
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.FinalPrice))
                    .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.DeliveryAddress.ZipCode))
                    .ForMember(dest => dest.ProductUrl, opt=> opt.MapFrom(src => src.OrderItems.First().Stock.Product.Pictures.First().Name));

                #endregion
            });

            IMapper mapper = config.CreateMapper();

            return services.AddSingleton(mapper);
        }
    }
}
