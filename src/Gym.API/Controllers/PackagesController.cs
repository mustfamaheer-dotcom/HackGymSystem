using Gym.Application.Common.DTOs;
using Gym.Application.Common.Interfaces;
using Gym.Application.Packages.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.API.Controllers;

[Authorize(Roles = "Owner")]
public class PackagesController : BaseController
{
    private readonly IPackageService _packageService;

    public PackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPackages(CancellationToken cancellationToken)
    {
        var result = await _packageService.GetAllAsync(cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve packages"));

        return Ok(ApiResponse<List<PackageDto>>.Ok(result.Data!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPackageById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _packageService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse.Fail(result.Message ?? "Package not found"));

        return Ok(ApiResponse<PackageDto>.Ok(result.Data!));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActivePackages(CancellationToken cancellationToken)
    {
        var result = await _packageService.GetActiveAsync(cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to retrieve active packages"));

        return Ok(ApiResponse<List<PackageDto>>.Ok(result.Data!));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPackages([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        var result = await _packageService.SearchAsync(searchTerm, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Search failed"));

        return Ok(ApiResponse<List<PackageDto>>.Ok(result.Data!));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest request, CancellationToken cancellationToken)
    {
        var result = await _packageService.CreateAsync(
            request.PackageName, request.DurationMonths, request.Price,
            request.FreePeriodMonths, request.MaxFreezeDays, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Fail(result.Message ?? "Failed to create package"));

        return Ok(ApiResponse<Guid>.Ok(result.Data!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] UpdatePackageRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest(ApiResponse.Fail("Route ID and body ID do not match"));

        var result = await _packageService.UpdateAsync(
            request.Id, request.PackageName, request.DurationMonths, request.Price,
            request.FreePeriodMonths, request.MaxFreezeDays, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to update package"));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(Guid id, CancellationToken cancellationToken)
    {
        var result = await _packageService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to delete package"));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivatePackage(Guid id, CancellationToken cancellationToken)
    {
        var result = await _packageService.ActivateAsync(id, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to activate package"));

        return Ok(ApiResponse.Ok(result.Message));
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivatePackage(Guid id, CancellationToken cancellationToken)
    {
        var result = await _packageService.DeactivateAsync(id, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Message ?? "Failed to deactivate package"));

        return Ok(ApiResponse.Ok(result.Message));
    }
}

public class CreatePackageRequest
{
    public string PackageName { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public decimal Price { get; set; }
    public int? FreePeriodMonths { get; set; }
    public int? MaxFreezeDays { get; set; }
}

public class UpdatePackageRequest
{
    public Guid Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public decimal Price { get; set; }
    public int? FreePeriodMonths { get; set; }
    public int? MaxFreezeDays { get; set; }
}
