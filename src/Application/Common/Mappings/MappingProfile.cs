using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DealClean.Application.Deals.Queries.GetDeals;
using DealClean.Domain.Entities;

namespace DealClean.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

        CreateMap<Deal, DealsVm>()
        .ForMember(dest => dest.Video, opt => opt.MapFrom(src => src.Video != null ? src.Video.Path : null))
            .ForMember(dest => dest.VideoAltText, opt => opt.MapFrom(src => src.Video != null ? src.Video.AltText : null));
        CreateMap<Hotel, HotelVm>();
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });

        }
    }
}