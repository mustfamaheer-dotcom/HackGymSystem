using Gym.API.Filters;
using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Application.Offers.DTOs;
using Gym.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Gym.API.Resources;

namespace Gym.API.Controllers;

[Authorize]
[Route("api/offers")]
public class OffersController : BaseController
{
    private readonly IOfferService _offerService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public OffersController(IOfferService offerService, IStringLocalizer<SharedResources> localizer)
    {
        _offerService = offerService;
        _localizer = localizer;
    }

    [HttpGet]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var result = await _offerService.GetAllAsync(page, pageSize, searchTerm, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to retrieve offers"]));

        return Ok(ApiResponse<PaginatedResult<OfferDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _offerService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Offer not found"]));

        return Ok(ApiResponse<OfferDto>.Ok(result.Data!));
    }

    [HttpGet("active")]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _offerService.GetActiveOffersAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to retrieve active offers"]));

        return Ok(ApiResponse<List<OfferDto>>.Ok(result.Data!));
    }

    [HttpGet("expired")]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> GetExpired(CancellationToken cancellationToken)
    {
        var result = await _offerService.GetExpiredOffersAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to retrieve expired offers"]));

        return Ok(ApiResponse<List<OfferDto>>.Ok(result.Data!));
    }

    [HttpGet("package/{packageId}")]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> GetByPackage(Guid packageId, CancellationToken cancellationToken)
    {
        var result = await _offerService.GetOffersByPackageAsync(packageId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to retrieve offers for package"]));

        return Ok(ApiResponse<List<OfferDto>>.Ok(result.Data!));
    }

    [HttpPost]
    [RequirePermission("Offers.Create")]
    public async Task<IActionResult> Create([FromBody] CreateOfferDto dto, CancellationToken cancellationToken)
    {
        var result = await _offerService.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to create offer"]));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    [RequirePermission("Offers.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOfferDto dto, CancellationToken cancellationToken)
    {
        dto.Id = id;
        var result = await _offerService.UpdateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to update offer"]));

        return Ok(ApiResponse.Ok());
    }

    [HttpDelete("{id}")]
    [RequirePermission("Offers.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _offerService.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to delete offer"]));

        return Ok(ApiResponse.Ok());
    }

    [HttpPost("apply")]
    [RequirePermission("Offers.View")]
    public async Task<IActionResult> Apply([FromBody] ApplyOfferDto dto, CancellationToken cancellationToken)
    {
        var result = await _offerService.ApplyOfferAsync(dto.OfferId, dto.PackageId, dto.PackagePrice, dto.PackageDurationMonths, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? _localizer["Failed to apply offer"]));

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
