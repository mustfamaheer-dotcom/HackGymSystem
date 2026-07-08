using System.Reflection;
using AutoMapper;
using Gym.Application.DailySessions.DTOs;
using Gym.Application.Leads.DTOs;
using Gym.Domain.Entities;

namespace Gym.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

        CreateMap<Lead, LeadDto>()
            .ForMember(d => d.InterestedPackageName, o => o.MapFrom(s => s.InterestedPackage != null ? s.InterestedPackage.Name : null))
            .ForMember(d => d.FollowUpCount, o => o.MapFrom(s => s.FollowUps.Count));

        CreateMap<LeadFollowUp, LeadFollowUpDto>();
        CreateMap<DailySession, DailySessionDto>()
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan != null ? s.Plan.Name : null))
            .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod.ToString()));
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping") ??
                           type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
