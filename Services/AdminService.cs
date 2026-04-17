using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories;
namespace PharmacyAPI.Services
{
    public class AdminService
    {
        private readonly AppDbContext _context;
        private readonly PharmacistRepository _pharmacistRepo;
        public AdminService(AppDbContext context, PharmacistRepository pharmacistRepo)
        {
            _context = context;
            _pharmacistRepo = pharmacistRepo;
        }
        public async Task<List<User>> GetAllUsersAsync()
        => await _context.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
        public async Task<List<Pharmacist>> GetAllPharmacistsAsync()
        => await _pharmacistRepo.GetAllAsync();
        public async Task<bool> UpdatePharmacistStatusAsync(int id, string status)
        {
            var pharmacist = await _pharmacistRepo.GetByIdAsync(id);
            if (pharmacist == null) return false;
            pharmacist.Status = status;
            pharmacist.ApprovedAt = status == "Approved" ? DateTime.UtcNow : null;
            await _pharmacistRepo.SaveChangesAsync();
            return true;
        }
        public async Task<List<Medicine>> GetAllMedicinesAsync()
        => await _context.Medicines.Include(m => m.Category).ToListAsync();
        public async Task<bool> UpdateMedicineAsync(int id, Medicine updated)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return false;
            medicine.Name = updated.Name;
            medicine.GenericName = updated.GenericName;
            medicine.Manufacturer = updated.Manufacturer;
            medicine.Description = updated.Description;
            medicine.Price = updated.Price;
            medicine.DosageForm = updated.DosageForm;
            medicine.Strength = updated.Strength;
            medicine.RequiresPrescription = updated.RequiresPrescription;
            medicine.IsActive = updated.IsActive;
            medicine.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Inventory>> GetAllInventoryAsync()
        => await _context.Inventories.Include(i => i.Medicine).ToListAsync();
        public async Task<bool> UpdateStockAsync(int medicineId, int quantity)
        {
            var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.MedicineId == medicineId);
            if (inventory == null) return false;
            inventory.QuantityInStock = quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
