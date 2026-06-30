using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gym.Application.Common.DTOs;
using Gym.Application.Payments.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Payments.Queries.GetAllPayments;

public record GetAllPaymentsQuery : PaginationRequest, IRequest<Result<PaginatedResult<PaymentDto>>>;

public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, Result<PaginatedResult<PaymentDto>>>
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IMapper _mapper;

    public GetAllPaymentsQueryHandler(IRepository<Payment> paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<PaymentDto>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var query = _paymentRepository.Query()
            .Include(p => p.Member)
            .OrderByDescending(p => p.PaymentDate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PaymentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<PaymentDto>>.Success(new PaginatedResult<PaymentDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
