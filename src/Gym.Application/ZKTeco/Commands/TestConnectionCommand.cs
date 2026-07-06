using Gym.Application.Common.Interfaces;
using Gym.Shared.Common;
using MediatR;

namespace Gym.Application.ZKTeco.Commands;

public record TestConnectionCommand : IRequest<Result<TestConnectionResult>>;

public class TestConnectionCommandHandler : IRequestHandler<TestConnectionCommand, Result<TestConnectionResult>>
{
    private readonly IZKTecoBridgeClient _bridge;

    public TestConnectionCommandHandler(IZKTecoBridgeClient bridge)
    {
        _bridge = bridge;
    }

    public async Task<Result<TestConnectionResult>> Handle(TestConnectionCommand request, CancellationToken cancellationToken)
    {
        var result = await _bridge.TestConnectionAsync(cancellationToken);
        return Result<TestConnectionResult>.Success(result);
    }
}
