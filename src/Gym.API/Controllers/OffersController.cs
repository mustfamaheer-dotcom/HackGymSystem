using Gym.Application.Common.DTOs;
using Gym.Application.Offers.Commands.CreateOffer;
using Gym.Shared.Enums;
using Gym.Application.Offers.Commands.DeleteOffer;
using Gym.Application.Offers.Commands.ToggleOfferStatus;
using Gym.Application.Offers.Commands.UpdateOffer;
using Gym.Application.Offers.DTOs;
using Gym.Application.Offers.Queries.GetActiveOffers;
using Gym.Application.Offers.Queries.GetAllOffers;
using Gym.Application.Offers.Queries.GetOfferById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
public class OffersController : BaseController
{
    private readonly IMediator _mediator;

    public OffersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOffersQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve offers"));

        return Ok(ApiResponse<PaginatedResult<OfferDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOfferByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Offer not found"));

        return Ok(ApiResponse<OfferDto>.Ok(result.Data!));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveOffersQuery(), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve active offers"));

        return Ok(ApiResponse<List<OfferDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOfferCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to create offer"));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOfferRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateOfferCommand(
            id, request.Title, request.Description, request.DiscountType,
            request.DiscountValue, request.StartDate, request.EndDate);

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to update offer"));

        return Ok(ApiResponse.Ok());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteOfferCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to delete offer"));

        return Ok(ApiResponse.Ok());
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ToggleStatus(Guid id, [FromBody] ToggleOfferStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ToggleOfferStatusCommand(id, request.IsActive), cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to toggle offer status"));

        return Ok(ApiResponse.Ok());
    }
}

public class UpdateOfferRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class ToggleOfferStatusRequest
{
    public bool IsActive { get; set; }
}
