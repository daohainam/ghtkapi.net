using AutoMapper;
using Ghtk.Api.Models;
using Ghtk.Repository.Abstractions.Entities;

namespace Ghtk.Api.AutoMapperProfiles;
public class OrderProfile: Profile
{
    public OrderProfile()
    {
        CreateMap<OrderProduct, Product>();

        CreateMap<SubmitOrderRequest, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.PickDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.TrackingId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.PickName, opt => opt.MapFrom(src => src.Order.PickName))
            .ForMember(dest => dest.PickAddress, opt => opt.MapFrom(src => src.Order.PickAddress))
            .ForMember(dest => dest.PickProvince, opt => opt.MapFrom(src => src.Order.PickProvince))
            .ForMember(dest => dest.PickDistrict, opt => opt.MapFrom(src => src.Order.PickDistrict))
            .ForMember(dest => dest.PickWard, opt => opt.MapFrom(src => src.Order.PickWard))
            .ForMember(dest => dest.PickTel, opt => opt.MapFrom(src => src.Order.PickTel))
            .ForMember(dest => dest.Tel, opt => opt.MapFrom(src => src.Order.Tel))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Order.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Order.Address))
            .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Order.Province))
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Order.District))
            .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Order.Ward))
            .ForMember(dest => dest.Hamlet, opt => opt.MapFrom(src => src.Order.Hamlet))
            .ForMember(dest => dest.IsFreeship, opt => opt.MapFrom(src => src.Order.IsFreeship))
            .ForMember(dest => dest.PickMoney, opt => opt.MapFrom(src => src.Order.PickMoney))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Order.Note))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Order.Value))
            .ForMember(dest => dest.Transport, opt => opt.MapFrom(src => src.Order.Transport))
            .ForMember(dest => dest.PickOption, opt => opt.MapFrom(src => src.Order.PickOption))
            .ForMember(dest => dest.DeliverOption, opt => opt.MapFrom(src => src.Order.DeliverOption))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
