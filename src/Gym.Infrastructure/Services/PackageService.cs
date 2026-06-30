using AutoMapper;
using Gym.Application.Common.Interfaces;
using Gym.Application.Packages.DTOs;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Services;

public class PackageService : IPackageService
{
    private readonly IPackageRepository _packageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PackageService(IPackageRepository packageRepository,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _packageRepository = packageRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<PackageDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var packages = await _packageRepository.Query()
            .OrderBy(p => p.PackageName)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<PackageDto>>(packages);
        return Result<List<PackageDto>>.Success(dtos);
    }

    public async Task<Result<List<PackageDto>>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var packages = await _packageRepository.Query()
            .Where(p => p.IsActive)
            .OrderBy(p => p.PackageName)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<PackageDto>>(packages);
        return Result<List<PackageDto>>.Success(dtos);
    }

    public async Task<Result<PackageDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetByIdAsync(id, cancellationToken);
        if (package is null)
            return Result<PackageDto>.Failure("Package not found");

        var dto = _mapper.Map<PackageDto>(package);
        return Result<PackageDto>.Success(dto);
    }

    public async Task<Result<List<PackageDto>>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var packages = await _packageRepository.SearchAsync(searchTerm, cancellationToken);
        var dtos = _mapper.Map<List<PackageDto>>(packages);
        return Result<List<PackageDto>>.Success(dtos);
    }

    public async Task<Result<Guid>> CreateAsync(string packageName, int durationMonths, decimal price,
        int? freePeriodMonths, int? maxFreezeDays, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(packageName))
            errors.Add("Package name is required");
        else if (packageName.Length > 200)
            errors.Add("Package name must not exceed 200 characters");

        if (durationMonths <= 0)
            errors.Add("Duration months must be greater than 0");

        if (price <= 0)
            errors.Add("Price must be greater than 0");

        if (freePeriodMonths < 0)
            errors.Add("Free period months cannot be negative");

        if (maxFreezeDays < 0)
            errors.Add("Max freeze days cannot be negative");

        if (errors.Count > 0)
            return Result<Guid>.Failure(string.Join("; ", errors));

        var existing = await _packageRepository.AnyAsync(p => p.PackageName == packageName, cancellationToken);
        if (existing)
            return Result<Guid>.Failure("A package with this name already exists");

        var package = new Domain.Entities.Package(packageName, durationMonths, price, freePeriodMonths, maxFreezeDays);
        await _packageRepository.AddAsync(package, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(package.Id, "Package created successfully");
    }

    public async Task<Result> UpdateAsync(Guid id, string packageName, int durationMonths, decimal price,
        int? freePeriodMonths, int? maxFreezeDays, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetByIdAsync(id, cancellationToken);
        if (package is null)
            return Result.Failure("Package not found");

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(packageName))
            errors.Add("Package name is required");
        else if (packageName.Length > 200)
            errors.Add("Package name must not exceed 200 characters");

        if (durationMonths <= 0)
            errors.Add("Duration months must be greater than 0");

        if (price <= 0)
            errors.Add("Price must be greater than 0");

        if (freePeriodMonths < 0)
            errors.Add("Free period months cannot be negative");

        if (maxFreezeDays < 0)
            errors.Add("Max freeze days cannot be negative");

        if (errors.Count > 0)
            return Result.Failure(string.Join("; ", errors));

        var duplicate = await _packageRepository.AnyAsync(p => p.PackageName == packageName && p.Id != id, cancellationToken);
        if (duplicate)
            return Result.Failure("A package with this name already exists");

        package.Update(packageName, durationMonths, price, freePeriodMonths, maxFreezeDays);
        _packageRepository.Update(package);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Package updated successfully");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetByIdAsync(id, cancellationToken);
        if (package is null)
            return Result.Failure("Package not found");

        _packageRepository.Delete(package);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Package deleted successfully");
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetByIdAsync(id, cancellationToken);
        if (package is null)
            return Result.Failure("Package not found");

        if (package.IsActive)
            return Result.Failure("Package is already active");

        await _packageRepository.ActivateAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Package activated successfully");
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetByIdAsync(id, cancellationToken);
        if (package is null)
            return Result.Failure("Package not found");

        if (!package.IsActive)
            return Result.Failure("Package is already inactive");

        await _packageRepository.DeactivateAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Package deactivated successfully");
    }
}
