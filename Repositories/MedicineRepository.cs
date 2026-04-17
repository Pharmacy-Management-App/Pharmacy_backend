using Microsoft.EntityFrameworkCore;
using Pharmacy_backend.DTOs;
using PharmacyAPI.Data;
using PharmacyAPI.Models;

namespace Pharmacy_backend.Repositories
{
    public interface IMedicineRepository
    {
        Task<PagedResult<Medicine>> GetAllAsync(MedicineQueryParams query);
        Task<Medicine?> GetByIdAsync(int id);
        Task<Medicine> CreateAsync(Medicine medicine, int initialStock, string batchNumber);
        Task<Medicine?> UpdateAsync(int id, Medicine medicine);
        Task<bool> DeleteAsync(int id);
        Task<List<Category>> GetCategoriesAsync();
        Task<bool> ExistsAsync(int id);
    }

    public class MedicineRepository : IMedicineRepository
    {
        private readonly AppDbContext _context;

        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── GET ALL with Search, Filter, Pagination ────────────
        public async Task<PagedResult<Medicine>> GetAllAsync(MedicineQueryParams query)
        {
            var dbQuery = _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Inventory)
                .AsQueryable();

            // Filter: active only
            if (query.IsActive.HasValue)
                dbQuery = dbQuery.Where(m => m.IsActive == query.IsActive.Value);

            // Filter: by category
            if (query.CategoryId.HasValue)
                dbQuery = dbQuery.Where(m => m.CategoryId == query.CategoryId.Value);

            // Filter: prescription required
            if (query.RequiresPrescription.HasValue)
                dbQuery = dbQuery.Where(m => m.RequiresPrescription == query.RequiresPrescription.Value);

            // Search: by name or generic name
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.ToLower();
                dbQuery = dbQuery.Where(m =>
                    m.Name.ToLower().Contains(search) ||
                    m.GenericName.ToLower().Contains(search) ||
                    m.Manufacturer.ToLower().Contains(search));
            }

            // Sort
            dbQuery = query.SortBy.ToLower() switch
            {
                "price" => query.SortOrder == "desc" ? dbQuery.OrderByDescending(m => m.Price) : dbQuery.OrderBy(m => m.Price),
                "createdat" => query.SortOrder == "desc" ? dbQuery.OrderByDescending(m => m.CreatedAt) : dbQuery.OrderBy(m => m.CreatedAt),
                _ => query.SortOrder == "desc" ? dbQuery.OrderByDescending(m => m.Name) : dbQuery.OrderBy(m => m.Name)
            };

            // Pagination
            var totalCount = await dbQuery.CountAsync();
            var items = await dbQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Medicine>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        // ── GET BY ID ──────────────────────────────────────────
        public async Task<Medicine?> GetByIdAsync(int id)
        {
            return await _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Inventory)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // ── CREATE ─────────────────────────────────────────────
        public async Task<Medicine> CreateAsync(Medicine medicine, int initialStock, string batchNumber)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            // Create inventory entry for new medicine
            var inventory = new Inventory
            {
                MedicineId = medicine.Id,
                QuantityInStock = initialStock,
                BatchNumber = batchNumber,
                LastUpdated = DateTime.UtcNow
            };
            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();

            return medicine;
        }

        // ── UPDATE ─────────────────────────────────────────────
        public async Task<Medicine?> UpdateAsync(int id, Medicine updated)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return null;

            medicine.CategoryId = updated.CategoryId;
            medicine.Name = updated.Name;
            medicine.GenericName = updated.GenericName;
            medicine.Manufacturer = updated.Manufacturer;
            medicine.Description = updated.Description;
            medicine.Price = updated.Price;
            medicine.DosageForm = updated.DosageForm;
            medicine.Strength = updated.Strength;
            medicine.RequiresPrescription = updated.RequiresPrescription;
            medicine.ImagePath = updated.ImagePath;
            medicine.IsActive = updated.IsActive;
            medicine.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return medicine;
        }

        // ── DELETE (Soft Delete) ───────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return false;

            medicine.IsActive = false;  // Soft delete
            medicine.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        // ── GET CATEGORIES ─────────────────────────────────────
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Medicines.AnyAsync(m => m.Id == id);
        }
    }
}