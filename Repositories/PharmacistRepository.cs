// Repositories/PharmacistRepository.cs
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories
{
    public class PharmacistRepository
    {
        private readonly AppDbContext _db;
        public PharmacistRepository(AppDbContext db) => _db = db;

        public async Task<List<Pharmacist>> GetAllAsync() =>
            await _db.Pharmacists.Include(p => p.User).ToListAsync();

        public async Task<List<Pharmacist>> GetPendingAsync() =>
            await _db.Pharmacists.Include(p => p.User)
                .Where(p => p.Status == "Pending").ToListAsync();

        public async Task<Pharmacist?> GetByIdAsync(int id) =>
            await _db.Pharmacists.Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Pharmacist?> GetByUserIdAsync(int userId) =>
            await _db.Pharmacists.Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

        public async Task UpdateAsync(Pharmacist pharmacist)
        {
            _db.Pharmacists.Update(pharmacist);
            await _db.SaveChangesAsync();
        }
    }
}
