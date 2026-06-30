using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Payments.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Payments.Queries.GetMemberPayments;

public record GetMemberPaymentsQuery(Guid MemberId) : IRequest<Result<List<PaymentDto>>>;

public class GetMemberPaymentsQueryHandler : IRequestHandler<GetMemberPaymentsQuery, Result<List<PaymentDto>>>
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IMapper _mapper;

    public GetMemberPaymentsQueryHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<PaymentDto>>> Handle(GetMemberPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.Query()
            .Include(p => p.Member)
            .Where(p => p.MemberId == request.MemberId)
            .OrderByDescending(p => p.PaymentDate)
            .ProjectTo<PaymentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<PaymentDto>>.Success(payments);
    }
}
