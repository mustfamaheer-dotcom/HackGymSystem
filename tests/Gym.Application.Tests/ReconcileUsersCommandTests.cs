using Gym.Application.Common.Interfaces;
using Gym.Application.ZKTeco.Commands;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Gym.Application.Tests;

public class ReconcileUsersCommandTests
{
    [Fact]
    public async Task Handle_NoMappings_ReturnsSuccess()
    {
        var bridgeMock = new Mock<IZKTecoBridgeClient>();
        var mappingRepoMock = new Mock<IDeviceMemberMappingRepository>();
        var subscriptionRepoMock = new Mock<IRepository<Subscription>>();
        var auditMock = new Mock<ISyncAuditService>();
        var loggerMock = new Mock<ILogger<ReconcileUsersCommandHandler>>();

        mappingRepoMock.Setup(r => r.GetAllActiveMappingsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DeviceMemberMapping>());

        var handler = new ReconcileUsersCommandHandler(bridgeMock.Object,
            mappingRepoMock.Object, subscriptionRepoMock.Object, auditMock.Object, loggerMock.Object);

        var result = await handler.Handle(new ReconcileUsersCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }
}
