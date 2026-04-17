// Services/AdminService.cs
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories;

namespace PharmacyAPI.Services
{
    public class AdminService
    {
        private readonly AppDbContext _db;
        private readonly PharmacistRepository _pharmacistRepo;

        public AdminService(AppDbContext db, PharmacistRepository pharmacistRepo)
        {
            _db = db;
            _pharmacistRepo = pharmacistRepo;
        }

        // ── Users ──────────────────────────────────────────────────────────
        public async Task<List<User>> GetAllUsersAsync() =>
            await _db.Users.Where(u => u.Role == "Customer").ToListAsync();

        public async Task<bool> ToggleUserStatusAsync(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return false;
            user.IsActive = !user.IsActive;
            await _db.SaveChangesAsync();
            return true;
        }

        // ── Pharmacists ────────────────────────────────────────────────────
        public async Task<List<Pharmacist>> GetAllPharmacistsAsync() =>
            await _pharmacistRepo.GetAllAsync();

        public async Task<List<Pharmacist>> GetPendingPharmacistsAsync() =>
            await _pharmacistRepo.GetPendingAsync();

        public async Task<bool> ApprovePharmacistAsync(int pharmacistId)
        {
            var ph = await _pharmacistRepo.GetByIdAsync(pharmacistId);
            if (ph == null) return false;
            ph.Status = "Approved";
            ph.ApprovedAt = DateTime.UtcNow;
            var user = await _db.Users.FindAsync(ph.UserId);
            if (user != null) user.IsActive = true;
            await _pharmacistRepo.UpdateAsync(ph);
            return true;
        }

        public async Task<bool> RejectPharmacistAsync(int pharmacistId)
        {
            var ph = await _pharmacistRepo.GetByIdAsync(pharmacistId);
            if (ph == null) return false;
            ph.Status = "Rejected";
            await _pharmacistRepo.UpdateAsync(ph);
            return true;
        }

        // ── Medicines ──────────────────────────────────────────────────────
        public async Task<List<Medicine>> GetAllMedicinesAsync() =>
            await _db.Medicines.Include(m => m.Category).ToListAsync();

        public async Task<Medicine> AddMedicineAsync(Medicine medicine)
        {
            medicine.CreatedAt = DateTime.UtcNow;
            await _db.Medicines.AddAsync(medicine);
            await _db.SaveChangesAsync();
            return medicine;
        }

        public async Task<bool> UpdateMedicineAsync(Medicine medicine)
        {
            var existing = await _db.Medicines.FindAsync(medicine.Id);
            if (existing == null) return false;
            existing.Name = medicine.Name;
            existing.GenericName = medicine.GenericName;
            existing.Price = medicine.Price;
            existing.IsActive = medicine.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleMedicineStatusAsync(int medicineId)
        {
            var m = await _db.Medicines.FindAsync(medicineId);
            if (m == null) return false;
            m.IsActive = !m.IsActive;
            await _db.SaveChangesAsync();
            return true;
        }

        // ── Inventory ──────────────────────────────────────────────────────
        public async Task<List<Inventory>> GetInventoryAsync() =>
            await _db.Inventories.Include(i => i.Medicine).ToListAsync();

        public async Task<bool> UpdateInventoryAsync(int medicineId, int quantity)
        {
            var inv = await _db.Inventories
                .FirstOrDefaultAsync(i => i.MedicineId == medicineId);
            if (inv == null) return false;
            inv.QuantityInStock = quantity;
            inv.LastUpdated = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        // ── Dashboard Stats ────────────────────────────────────────────────
        public async Task<object> GetDashboardStatsAsync()
        {
            return new
            {
                TotalUsers = await _db.Users.CountAsync(u => u.Role == "Customer"),
                TotalPharmacists = await _db.Pharmacists.CountAsync(),
                PendingPharmacists = await _db.Pharmacists
                    .CountAsync(p => p.Status == "Pending"),
                TotalMedicines = await _db.Medicines.CountAsync(),
                TotalOrders = await _db.Orders.CountAsync(),
                PendingPrescriptions = await _db.Prescriptions
                    .CountAsync(p => p.Status == "Pending")
            };
        }
    }
}
