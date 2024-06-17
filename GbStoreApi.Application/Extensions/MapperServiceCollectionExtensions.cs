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
using GbStoreApi.Domain.Dto.UserAddresses;
using GbStoreApi.Domain.Dto.Users;
using GbStoreApi.Domain.enums;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Models.Purchases;
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
                configuration.CreateProjection<Product, DisplayProductDto>()
                    .ForMember(member => member.RealPrice, map => map.MapFrom(x => x.UnitaryPrice))
                    .ForMember(member => member.PhotoUrlId, map => map.MapFrom(x => x.Pictures.FirstOrDefault().Name))
                    .ForMember(member => member.Category, map => map.MapFrom(x => x.Category.Name))
                    .ForMember(member => member.Colors, map => map.MapFrom(src => src.Stocks.Select(stocks => stocks.Color!.Name).Distinct()))
                    .ForMember(member => member.Sizes, map => map.MapFrom(src => src.Stocks.Select(stocks => stocks.Size!.Name).Distinct()));

                configuration.CreateMap<Product, ProductSpecificationsDto>()
                    .ForMember(member => member.RealPrice, map => map.MapFrom(x => x.UnitaryPrice))
                    .ForMember(member => member.ProductPictureIds, map => map.MapFrom(x => x.Pictures.Select(y => y.Name)))
                    .ForMember(member => member.Category, map => map.MapFrom(x => x.Category!.Name))
                    .ReverseMap();
                configuration.CreateMap<ProductStock, StockDto>()
                    .ForMember(member => member.StockId, map => map.MapFrom(x => x.Id))
                    .ForMember(member => member.Amount, map => map.MapFrom(x => x.Count))
                    .ReverseMap();

                configuration.CreateMap<CreateProductDto, Product>();
                #endregion

                #region [Stock]
                configuration.CreateProjection<ProductStock, StockAvaliableByIdDto>()
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.Id));
                #endregion

                #region [Brand]
                configuration.CreateMap<Brand, DisplayBrandDto>().ReverseMap();
                #endregion

                #region [User]
                configuration.CreateMap<DisplayUserDto, User>();
                configuration.CreateMap<User, DisplayUserDto>()
                .ForMember(dest => dest.TypeOfUser, opt => opt.MapFrom(src => src.TypeOfUser.ToString()));
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
                    .IncludeBase<BaseAddressDto, Address>();

                configuration.CreateMap<UpdateAddressDto, Address>();
                #endregion

                #region UserAddress
                configuration.CreateMap<BaseAddressDto, UserAddress>();

                configuration.CreateMap<CreateAddressDto, UserAddress>()
                    .IncludeBase<BaseAddressDto, UserAddress>();

                configuration.CreateMap<CreateUserAddressByAddress, UserAddress>()
                    .IncludeMembers(dest => dest.Address)
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                configuration.CreateMap<UserAddress, DisplayAddressDto>()
                    .IncludeMembers(src => src.Address)
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId));

                configuration.CreateMap<UpdateAddressDto, UserAddress>();
                #endregion

                #region [Purchase]
                configuration.CreateMap<CreateOrderItemDto, OrderItems>();

                configuration.CreateMap<BaseBuyProductDto, Purchase>();

                configuration.CreateMap<OrderItems, DisplayPurchaseItem>()
                    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Stock.Product.Name))
                    .ForMember(dest => dest.ProductUrl, opt => opt.MapFrom(src => src.Stock.Product.Pictures.First().Name))
                    .ForMember(dest => dest.UnitaryPrice, opt => opt.MapFrom(src => src.ProductStockPrice))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.ProductCount));

                configuration.CreateMap<BuyProductDto, Purchase>()
                    .IncludeBase<BaseBuyProductDto, Purchase>()
                    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                    .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.Items.Sum(item => item.ProductStockPrice * item.ProductCount)))
                    .ForMember(dest => dest.TypeOfPayment, opt => opt.MapFrom(src => src.PaymentMethod));

                configuration.CreateMap<Purchase, PurchaseSpecificationDto>()
                    .ForMember(dest => dest.BoughterId, opt => opt.MapFrom(src =>
                        src.ShippingPurchase != null ?
                        src.ShippingPurchase.UserOwnerAddress.UserId :
                        src.StorePickupPurchase.UserBuyerId))
                    .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src =>
                    src.ShippingPurchase != null ?
                        src.ShippingPurchase.UserOwnerAddress.Address.ZipCode :
                        src.StorePickupPurchase.StoreAddress.ZipCode
                    ))
                    .ForMember(dest => dest.PurchaseId, opt => opt.MapFrom(src => src.Id))
                    ;

                #endregion

                #region [StorePickupPurchase]
                configuration.CreateMap<BuyProductDto, StorePickupPurchase>()
                    .ForMember(dest => dest.Purchase, opt => opt.MapFrom(src => src));
                #endregion

                #region [ShippingPurchase]
                configuration.CreateMap<BuyProductDto, ShippingPurchase>()
                    .ForMember(dest => dest.Purchase, opt => opt.MapFrom(src => src));
                #endregion
            });

            IMapper mapper = config.CreateMapper();

            return services.AddSingleton(mapper);
        }
    }
}
