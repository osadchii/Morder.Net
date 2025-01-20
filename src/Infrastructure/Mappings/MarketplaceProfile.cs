using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.MediatR.ChangeTracking.Prices.Commands;
using Infrastructure.MediatR.ChangeTracking.Stocks.Commands;
using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using Infrastructure.MediatR.Marketplaces.Kuper.Commands;
using Infrastructure.MediatR.Marketplaces.Meso.Commands;
using Infrastructure.MediatR.Marketplaces.Ozon.Commands;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.MediatR.Marketplaces.YandexMarket.Commands;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Infrastructure.Models.Marketplaces.Meso;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Marketplaces.YandexMarket;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;

namespace Infrastructure.Mappings;

public class MarketplaceProfile : Profile
{
    public MarketplaceProfile()
    {
        CreateMap<Marketplace, SberMegaMarketDto>()
            .ForMember(m => m.ProductTypes,
                opt
                    => opt.MapFrom(e => e.ProductTypes.FromJson<List<ProductType>>()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings.FromJson<SberMegaMarketSettings>()))
            .ForMember(m => m.WarehouseExternalId,
                opt =>
                    opt.MapFrom(e => e.Warehouse.ExternalId))
            .ForMember(m => m.PriceTypeExternalId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.ExternalId));

        CreateMap<UpdateSberMegaMarketRequest, Marketplace>()
            .ForMember(m => m.ProductTypes,
                opt =>
                    opt.MapFrom(e => e.ProductTypes!.ToJson()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings!.ToJson()))
            .ForMember(m => m.Type,
                opt =>
                    opt.MapFrom(e => MarketplaceType.SberMegaMarket))
            .ForMember(m => m.PriceTypeId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.Id))
            .ForMember(m => m.WarehouseId,
                opt =>
                    opt.MapFrom(e => e.Warehouse!.Id))
            .ForMember(m => m.Warehouse,
                opt => opt.Ignore())
            .ForMember(m => m.PriceType,
                opt => opt.Ignore());

        CreateMap<Marketplace, OzonDto>()
            .ForMember(m => m.ProductTypes,
                opt
                    => opt.MapFrom(e => e.ProductTypes.FromJson<List<ProductType>>()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings.FromJson<OzonSettings>()))
            .ForMember(m => m.WarehouseExternalId,
                opt =>
                    opt.MapFrom(e => e.Warehouse.ExternalId))
            .ForMember(m => m.PriceTypeExternalId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.ExternalId));

        CreateMap<UpdateOzonRequest, Marketplace>()
            .ForMember(m => m.ProductTypes,
                opt =>
                    opt.MapFrom(e => e.ProductTypes!.ToJson()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings!.ToJson()))
            .ForMember(m => m.Type,
                opt =>
                    opt.MapFrom(e => MarketplaceType.Ozon))
            .ForMember(m => m.PriceTypeId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.Id))
            .ForMember(m => m.WarehouseId,
                opt =>
                    opt.MapFrom(e => e.Warehouse!.Id))
            .ForMember(m => m.Warehouse,
                opt => opt.Ignore())
            .ForMember(m => m.PriceType,
                opt => opt.Ignore());

        CreateMap<Marketplace, MesoDto>()
            .ForMember(m => m.ProductTypes,
                opt
                    => opt.MapFrom(e => e.ProductTypes.FromJson<List<ProductType>>()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings.FromJson<MesoSettings>()))
            .ForMember(m => m.WarehouseExternalId,
                opt =>
                    opt.MapFrom(e => e.Warehouse.ExternalId))
            .ForMember(m => m.PriceTypeExternalId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.ExternalId));

        CreateMap<Marketplace, KuperDto>()
            .ForMember(m => m.ProductTypes,
                opt
                    => opt.MapFrom(e => e.ProductTypes.FromJson<List<ProductType>>()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings.FromJson<KuperSettings>()))
            .ForMember(m => m.WarehouseExternalId,
                opt =>
                    opt.MapFrom(e => e.Warehouse.ExternalId))
            .ForMember(m => m.PriceTypeExternalId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.ExternalId));

        CreateMap<UpdateMesoRequest, Marketplace>()
            .ForMember(m => m.ProductTypes,
                opt =>
                    opt.MapFrom(e => e.ProductTypes!.ToJson()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings!.ToJson()))
            .ForMember(m => m.Type,
                opt =>
                    opt.MapFrom(e => MarketplaceType.Meso))
            .ForMember(m => m.PriceTypeId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.Id))
            .ForMember(m => m.WarehouseId,
                opt =>
                    opt.MapFrom(e => e.Warehouse!.Id))
            .ForMember(m => m.Warehouse,
                opt => opt.Ignore())
            .ForMember(m => m.PriceType,
                opt => opt.Ignore());

        CreateMap<UpdateKuperRequest, Marketplace>()
            .ForMember(m => m.ProductTypes,
                opt =>
                    opt.MapFrom(e => e.ProductTypes!.ToJson()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings!.ToJson()))
            .ForMember(m => m.Type,
                opt =>
                    opt.MapFrom(e => MarketplaceType.Meso))
            .ForMember(m => m.PriceTypeId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.Id))
            .ForMember(m => m.WarehouseId,
                opt =>
                    opt.MapFrom(e => e.Warehouse!.Id))
            .ForMember(m => m.Warehouse,
                opt => opt.Ignore())
            .ForMember(m => m.PriceType,
                opt => opt.Ignore());

        CreateMap<Marketplace, YandexMarketDto>()
            .ForMember(m => m.ProductTypes,
                opt
                    => opt.MapFrom(e => e.ProductTypes.FromJson<List<ProductType>>()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings.FromJson<YandexMarketSettings>()))
            .ForMember(m => m.WarehouseExternalId,
                opt =>
                    opt.MapFrom(e => e.Warehouse.ExternalId))
            .ForMember(m => m.PriceTypeExternalId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.ExternalId));

        CreateMap<UpdateYandexMarketRequest, Marketplace>()
            .ForMember(m => m.ProductTypes,
                opt =>
                    opt.MapFrom(e => e.ProductTypes!.ToJson()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings!.ToJson()))
            .ForMember(m => m.Type,
                opt =>
                    opt.MapFrom(e => MarketplaceType.YandexMarket))
            .ForMember(m => m.PriceTypeId,
                opt =>
                    opt.MapFrom(e => e.PriceType!.Id))
            .ForMember(m => m.WarehouseId,
                opt =>
                    opt.MapFrom(e => e.Warehouse!.Id))
            .ForMember(m => m.Warehouse,
                opt => opt.Ignore())
            .ForMember(m => m.PriceType,
                opt => opt.Ignore());

        CreateMap<TrackPriceChangeRequest, PriceChange>();
        CreateMap<TrackStockChangeRequest, StockChange>();
        CreateMap<SetMarketplaceProductSettingsRequest, MarketplaceProductSetting>();
    }
}