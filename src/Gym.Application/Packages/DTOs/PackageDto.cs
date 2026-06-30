using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;

namespace Gym.Application.Packages.DTOs;

public class PackageDto : IMapFrom<Package>
{
    public Guid Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public decimal Price { get; set; }
    public int? FreePeriodMonths { get; set; }
    public int? MaxFreezeDays { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Package, PackageDto>();
    }
}
