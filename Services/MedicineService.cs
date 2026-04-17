using Pharmacy_backend.DTOs;
using Pharmacy_backend.Repositories;
using PharmacyAPI.Models;

namespace Pharmacy_backend.Services
{
    public interface IMedicineService
    {
        Task<PagedResult<MedicineDto>> GetAllMedicinesAsync(MedicineQueryParams query);
        Task<MedicineDto?> GetMedicineByIdAsync(int id);
        Task<MedicineDto> CreateMedicineAsync(CreateMedicineDto dto);
        Task<MedicineDto?> UpdateMedicineAsync(int id, UpdateMedicineDto dto);
        Task<bool> DeleteMedicineAsync(int id);
        Task<List<CategoryDto>> GetCategoriesAsync();
    }

    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _repository;

        public MedicineService(IMedicineRepository repository)
        {
            _repository = repository;
        }

        // ── MAP Model → DTO ────────────────────────────────────
        private static MedicineDto MapToDto(Medicine m) => new()
        {
            Id = m.Id,
            CategoryId = m.CategoryId,
            CategoryName = m.Category?.Name ?? string.Empty,
            Name = m.Name,
            GenericName = m.GenericName,
            Manufacturer = m.Manufacturer,
            Description = m.Description,
            Price = m.Price,
            DosageForm = m.DosageForm,
            Strength = m.Strength,
            RequiresPrescription = m.RequiresPrescription,
            ImagePath = m.ImagePath,
            IsActive = m.IsActive,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt,
            QuantityInStock = m.Inventory?.QuantityInStock ?? 0
        };

        // ── GET ALL ────────────────────────────────────────────
        public async Task<PagedResult<MedicineDto>> GetAllMedicinesAsync(MedicineQueryParams query)
        {
            var result = await _repository.GetAllAsync(query);
            return new PagedResult<MedicineDto>
            {
                Items = result.Items.Select(MapToDto).ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        // ── GET BY ID ──────────────────────────────────────────
        public async Task<MedicineDto?> GetMedicineByIdAsync(int id)
        {
            var medicine = await _repository.GetByIdAsync(id);
            return medicine == null ? null : MapToDto(medicine);
        }

        // ── CREATE ─────────────────────────────────────────────
        public async Task<MedicineDto> CreateMedicineAsync(CreateMedicineDto dto)
        {
            var medicine = new Medicine
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                GenericName = dto.GenericName,
                Manufacturer = dto.Manufacturer,
                Description = dto.Description,
                Price = dto.Price,
                DosageForm = dto.DosageForm,
                Strength = dto.Strength,
                RequiresPrescription = dto.RequiresPrescription,
                ImagePath = dto.ImagePath,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(medicine, dto.InitialStock, dto.BatchNumber);
            var full = await _repository.GetByIdAsync(created.Id);
            return MapToDto(full!);
        }

        // ── UPDATE ─────────────────────────────────────────────
        public async Task<MedicineDto?> UpdateMedicineAsync(int id, UpdateMedicineDto dto)
        {
            var updated = new Medicine
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                GenericName = dto.GenericName,
                Manufacturer = dto.Manufacturer,
                Description = dto.Description,
                Price = dto.Price,
                DosageForm = dto.DosageForm,
                Strength = dto.Strength,
                RequiresPrescription = dto.RequiresPrescription,
                ImagePath = dto.ImagePath,
                IsActive = dto.IsActive
            };

            var result = await _repository.UpdateAsync(id, updated);
            if (result == null) return null;

            var full = await _repository.GetByIdAsync(id);
            return MapToDto(full!);
        }

        // ── DELETE ─────────────────────────────────────────────
        public async Task<bool> DeleteMedicineAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        // ── GET CATEGORIES ─────────────────────────────────────
        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var cats = await _repository.GetCategoriesAsync();
            return cats.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }

    // Category DTO (used in medicine module)
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}