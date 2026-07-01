using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Application.Offers.DTOs;
using Gym.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
[Route("api/offers")]
public class OffersController : BaseController
{
    private readonly IOfferService _offerService;

    public OffersController(IOfferService offerService)
    {
        _offerService = offerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var result = await _offerService.GetAllAsync(page, pageSize, searchTerm, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve offers"));

        return Ok(ApiResponse<PaginatedResult<OfferDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _offerService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Offer not found"));

        return Ok(ApiResponse<OfferDto>.Ok(result.Data!));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _offerService.GetActiveOffersAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve active offers"));

        return Ok(ApiResponse<List<OfferDto>>.Ok(result.Data!));
    }

    [HttpGet("expired")]
    public async Task<IActionResult> GetExpired(CancellationToken cancellationToken)
    {
        var result = await _offerService.GetExpiredOffersAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve expired offers"));

        return Ok(ApiResponse<List<OfferDto>>.Ok(result.Data!));
    }

    [HttpGet("package/{packageId}")]
    public async Task<IActionResult> GetByPackage(Guid packageId, CancellationToken cancellationToken)
    {
        var result = await _offerService.GetOffersByPackageAsync(packageId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve offers for package"));

        return Ok(ApiResponse<List<OfferDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOfferDto dto, CancellationToken cancellationToken)
    {
        var result = await _offerService.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to create offer"));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOfferDto dto, CancellationToken cancellationToken)
    {
        dto.Id = id;
        var result = await _offerService.UpdateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to update offer"));

        return Ok(ApiResponse.Ok());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _offerService.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to delete offer"));

        return Ok(ApiResponse.Ok());
    }

    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] ApplyOfferDto dto, CancellationToken cancellationToken)
    {
        var result = await _offerService.ApplyOfferAsync(dto.OfferId, dto.PackageId, dto.PackagePrice, dto.PackageDurationMonths, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to apply offer"));

        return Ok(ApiResponse<AppliedOfferDto>.Ok(result.Data!));
    }
}

public class ApplyOfferDto
{
    public Guid OfferId { get; set; }
    public Guid? PackageId { get; set; }
    public decimal? PackagePrice { get; set; }
    public int? PackageDurationMonths { get; set; }
}
