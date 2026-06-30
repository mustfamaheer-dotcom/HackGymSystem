using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Payments.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Payments.Queries.GetPaymentById;

public record GetPaymentByIdQuery(Guid Id) : IRequest<Result<PaymentDto>>;

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDto>>
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentByIdQueryHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaymentDto>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.Query()
            .Include(p => p.Member)
            .Include(p => p.Employee)
            .ProjectTo<PaymentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (payment == null)
            return Result<PaymentDto>.Failure("Payment not found");

        return Result<PaymentDto>.Success(payment);
    }
}
