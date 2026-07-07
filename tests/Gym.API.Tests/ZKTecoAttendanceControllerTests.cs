using Gym.API.Controllers;
using Gym.API.Hubs;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Moq;

namespace Gym.API.Tests;

public class ZKTecoAttendanceControllerTests
{
    private readonly Mock<IDeviceMemberMappingRepository> _mappingRepoMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IRepository<Device>> _deviceRepoMock;
    private readonly Mock<IRepository<Attendance>> _attendanceRepoMock;
    private readonly Mock<IRepository<Subscription>> _subscriptionRepoMock;
    private readonly Mock<IHubContext<AttendanceHub>> _hubMock;
    private readonly Mock<IZKTecoBridgeClient> _bridgeMock;
    private readonly ZKTecoSettings _settings;
    private readonly ZKTecoAttendanceController _controller;

    public ZKTecoAttendanceControllerTests()
    {
        _mappingRepoMock = new Mock<IDeviceMemberMappingRepository>();
        _mediatorMock = new Mock<IMediator>();
        _deviceRepoMock = new Mock<IRepository<Device>>();
        _attendanceRepoMock = new Mock<IRepository<Attendance>>();
        _subscriptionRepoMock = new Mock<IRepository<Subscription>>();
        _hubMock = new Mock<IHubContext<AttendanceHub>>();
        _bridgeMock = new Mock<IZKTecoBridgeClient>();
        _settings = new ZKTecoSettings { ApiKey = "test-key-123" };

        var optionsMock = new Mock<IOptions<ZKTecoSettings>>();
        optionsMock.Setup(o => o.Value).Returns(_settings);

        _controller = new ZKTecoAttendanceController(
            _mediatorMock.Object,
            _mappingRepoMock.Object,
            _deviceRepoMock.Object,
            optionsMock.Object,
            _attendanceRepoMock.Object,
            _subscriptionRepoMock.Object,
            _hubMock.Object,
            _bridgeMock.Object
        );

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task PushAttendance_NoApiKey_Returns401()
    {
        var result = await _controller.PushAttendance(new DeviceAttendancePushRequest
        {
            EnrollmentId = "123",
            Timestamp = DateTime.UtcNow
        }, CancellationToken.None);

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task PushAttendance_WrongApiKey_Returns401()
    {
        _controller.Request.Headers["X-API-Key"] = "wrong-key";

        var result = await _controller.PushAttendance(new DeviceAttendancePushRequest
        {
            EnrollmentId = "123",
            Timestamp = DateTime.UtcNow
        }, CancellationToken.None);

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task PushAttendance_NoMapping_Returns404()
    {
        _controller.Request.Headers["X-API-Key"] = "test-key-123";
        _mappingRepoMock.Setup(r => r.GetByEnrollmentIdAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync((DeviceMemberMapping?)null);

        var result = await _controller.PushAttendance(new DeviceAttendancePushRequest
        {
            EnrollmentId = "123",
            Timestamp = DateTime.UtcNow
        }, CancellationToken.None);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task PushAttendance_ValidMapping_ReturnsOk()
    {
        _controller.Request.Headers["X-API-Key"] = "test-key-123";
        var mapping = new DeviceMemberMapping("123", Guid.NewGuid(), BiometricType.Fingerprint);
        _mappingRepoMock.Setup(r => r.GetByEnrollmentIdAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(mapping);

        var result = await _controller.PushAttendance(new DeviceAttendancePushRequest
        {
            EnrollmentId = "123",
            Timestamp = DateTime.UtcNow
        }, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }
}
