using AutoMapper;
using GbStoreApi.Domain.Dto.Address;
using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Categories;
using GbStoreApi.Domain.Dto.Colors;
using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Purchases;
using GbStoreApi.Domain.Dto.Sizes;
using GbStoreApi.Domain.Dto.Stocks;
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
                configuration.CreateMap<Address, DisplayAddressDto>();
                configuration.CreateMap<CreateAddressDto, Address>();
                configuration.CreateMap<UpdateAddressDto, Address>();
                #endregion

                #region [Product]
                configuration.CreateMap<CreateProductDto, Product>();
                #endregion

                #region [Purchase]
                configuration.CreateMap<BuyProductDto, Purchase>()
                    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                    .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.Items.Sum(x => x.ProductStockPrice)));
                configuration.CreateMap<CreateOrderItemDto, OrderItems>();
                #endregion
            });

            IMapper mapper = config.CreateMapper();

            return services.AddSingleton(mapper);
        }
    }
}
