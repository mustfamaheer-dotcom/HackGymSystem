using Gym.API.Controllers;
using Gym.API.Hubs;
using Gym.Application.Attendances.Commands.CheckIn;
using Gym.Application.Common.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
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
    private readonly Mock<IZKTecoBridgeClient> _bridgeMock;
    private readonly ZKTecoAttendanceController _controller;

    public ZKTecoAttendanceControllerTests()
    {
        _mappingRepoMock = new Mock<IDeviceMemberMappingRepository>();
        _mediatorMock = new Mock<IMediator>();
        _deviceRepoMock = new Mock<IRepository<Device>>();
        _attendanceRepoMock = new Mock<IRepository<Attendance>>();
        _subscriptionRepoMock = new Mock<IRepository<Subscription>>();
        _bridgeMock = new Mock<IZKTecoBridgeClient>();

        var hubMock = new Mock<IHubContext<AttendanceHub>>();
        var clientProxyMock = new Mock<IClientProxy>();
        var clientsMock = new Mock<IHubClients>();
        clientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);
        hubMock.Setup(h => h.Clients).Returns(clientsMock.Object);

        _controller = new ZKTecoAttendanceController(
            _mediatorMock.Object,
            _mappingRepoMock.Object,
            _deviceRepoMock.Object,
            Mock.Of<IOptions<ZKTecoSettings>>(o => o.Value == new ZKTecoSettings()),
            _attendanceRepoMock.Object,
            _subscriptionRepoMock.Object,
            hubMock.Object,
            _bridgeMock.Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task PushAttendance_NoMapping_Returns404()
    {
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
        var memberId = Guid.NewGuid();
        var mapping = new DeviceMemberMapping(memberId, "123", BiometricType.Fingerprint);
        _mappingRepoMock.Setup(r => r.GetByEnrollmentIdAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(mapping);
        _subscriptionRepoMock.Setup(r => r.AnyAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<Subscription, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mediatorMock.Setup(m => m.Send(It.IsAny<CheckInCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Success(Guid.NewGuid()));

        var result = await _controller.PushAttendance(new DeviceAttendancePushRequest
        {
            EnrollmentId = "123",
            Timestamp = DateTime.UtcNow,
            Direction = 0
        }, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }
}
