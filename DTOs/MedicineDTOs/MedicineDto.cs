using System.ComponentModel.DataAnnotations;

namespace Pharmacy_backend.DTOs
{
    // ── Response DTO ──────────────────────────────────────────
    public class MedicineDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string DosageForm { get; set; } = string.Empty;
        public string Strength { get; set; } = string.Empty;
        public bool RequiresPrescription { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Stock info from Inventory
        public int QuantityInStock { get; set; }
        public bool InStock => QuantityInStock > 0;
    }

    // ── Create DTO ────────────────────────────────────────────
    public class CreateMedicineDto
    {
        [Required] public int CategoryId { get; set; }
        [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
        [MaxLength(200)] public string GenericName { get; set; } = string.Empty;
        [MaxLength(200)] public string Manufacturer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required, Range(0.01, 99999.99)] public decimal Price { get; set; }
        [MaxLength(100)] public string DosageForm { get; set; } = string.Empty;
        [MaxLength(100)] public string Strength { get; set; } = string.Empty;
        public bool RequiresPrescription { get; set; } = false;
        public string? ImagePath { get; set; }
        public int InitialStock { get; set; } = 0;
        public string BatchNumber { get; set; } = string.Empty;
    }

    // ── Update DTO ────────────────────────────────────────────
    public class UpdateMedicineDto
    {
        [Required] public int CategoryId { get; set; }
        [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
        [MaxLength(200)] public string GenericName { get; set; } = string.Empty;
        [MaxLength(200)] public string Manufacturer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required, Range(0.01, 99999.99)] public decimal Price { get; set; }
        [MaxLength(100)] public string DosageForm { get; set; } = string.Empty;
        [MaxLength(100)] public string Strength { get; set; } = string.Empty;
        public bool RequiresPrescription { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // ── Pagination Wrapper ─────────────────────────────────────
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    // ── Query Params ──────────────────────────────────────────
    public class MedicineQueryParams
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public bool? RequiresPrescription { get; set; }
        public bool? IsActive { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
    }
}
