using AutoMapper;
using Gym.Application.Common.Mappings;
using Gym.Domain.Entities;
using Gym.Shared.Enums;

namespace Gym.Application.Devices.DTOs;

public class DeviceDto : IMapFrom<Device>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? FirmwareVersion { get; set; }
    public bool IsActive { get; set; }
    public DeviceStatus Status { get; set; }
    public DateTime? LastConnectedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Device, DeviceDto>();
    }
}
