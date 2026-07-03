using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Gym.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Leads.Commands.ConvertToMember;

public record ConvertToMemberCommand(Guid LeadId, Guid PlanId, decimal AmountPaid, PaymentMethod PaymentMethod) : IRequest<Result<Guid>>;

public class ConvertToMemberCommandHandler : IRequestHandler<ConvertToMemberCommand, Result<Guid>>
{
    private readonly IRepository<Lead> _leadRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<MembershipPlan> _planRepo;
    private readonly IRepository<Domain.Entities.Subscription> _subscriptionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ConvertToMemberCommandHandler(
        IRepository<Lead> leadRepo,
        IRepository<Member> memberRepo,
        IRepository<MembershipPlan> planRepo,
        IRepository<Domain.Entities.Subscription> subscriptionRepo,
        IUnitOfWork unitOfWork)
    {
        _leadRepo = leadRepo;
        _memberRepo = memberRepo;
        _planRepo = planRepo;
        _subscriptionRepo = subscriptionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ConvertToMemberCommand request, CancellationToken cancellationToken)
    {
        var lead = await _leadRepo.Query()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(l => l.Id == request.LeadId, cancellationToken);
        if (lead == null)
            return Result<Guid>.Failure("Lead not found");

        var plan = await _planRepo.GetByIdAsync(request.PlanId, cancellationToken);
        if (plan == null)
            return Result<Guid>.Failure("Plan not found");

        var now = DateTime.UtcNow;

        // Generate code
        var lastMember = await _memberRepo.Query()
            .OrderByDescending(m => m.Code)
            .FirstOrDefaultAsync(cancellationToken);
        var code = (lastMember?.Code ?? 0) + 1;

        // Generate receipt number
        var lastReceipt = await _memberRepo.Query()
            .OrderByDescending(m => m.ReceiptNumber)
            .Select(m => m.ReceiptNumber)
            .FirstOrDefaultAsync(cancellationToken);
        int nextNum = 1;
        if (lastReceipt != null && lastReceipt.StartsWith("REC-"))
        {
            int.TryParse(lastReceipt[4..], out nextNum);
            nextNum++;
        }
        var receiptNumber = $"REC-{nextNum:D6}";

        var member = new Member(receiptNumber, lead.Name, lead.Phone, now)
        {
            Code = code,
            PackageId = plan.Id
        };
        await _memberRepo.AddAsync(member, cancellationToken);

        // Create subscription
        var expirationDate = now.AddDays(plan.DurationDays);
        var subscription = new Domain.Entities.Subscription(
            receiptNumber, member.Id, plan.Id, plan.Price,
            request.AmountPaid, request.PaymentMethod, now, expirationDate)
        {
            AdminSignature = "Converted from Lead"
        };
        await _subscriptionRepo.AddAsync(subscription, cancellationToken);

        // Mark lead as converted
        lead.MarkConverted();
        _leadRepo.Update(lead);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(member.Id, "Lead converted to member successfully");
    }
}