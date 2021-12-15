using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.Marketplaces;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Products;

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
                    opt.MapFrom(e => e.Settings.FromJson<SberMegaMarketSettings>()));

        CreateMap<UpdateSberMegaMarketRequest, Marketplace>()
            .ForMember(m => m.ProductTypes,
                opt =>
                    opt.MapFrom(e => e.ProductTypes!.ToJson()))
            .ForMember(m => m.Settings,
                opt =>
                    opt.MapFrom(e => e.Settings!.ToJson()))
            .ForMember(m => m.Type,
                opt =>
                    opt.NullSubstitute(MarketplaceType.SberMegaMarket))
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
    }
}