// Repositories/PrescriptionRepository.cs
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories
{
    public class PrescriptionRepository
    {
        private readonly AppDbContext _db;
        public PrescriptionRepository(AppDbContext db) => _db = db;

        public async Task<List<Prescription>> GetAllAsync() =>
            await _db.Prescriptions.Include(p => p.User).ToListAsync();

        public async Task<List<Prescription>> GetPendingAsync() =>
            await _db.Prescriptions.Include(p => p.User)
                .Where(p => p.Status == "Pending").ToListAsync();

        public async Task<List<Prescription>> GetApprovedAsync() =>
            await _db.Prescriptions.Include(p => p.User)
                .Where(p => p.Status == "Approved").ToListAsync();

        public async Task<Prescription?> GetByIdAsync(int id) =>
            await _db.Prescriptions.Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Prescription>> GetByUserIdAsync(int userId) =>
            await _db.Prescriptions.Where(p => p.UserId == userId).ToListAsync();

        public async Task AddAsync(Prescription prescription)
        {
            await _db.Prescriptions.AddAsync(prescription);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prescription prescription)
        {
            _db.Prescriptions.Update(prescription);
            await _db.SaveChangesAsync();
        }
    }
}
