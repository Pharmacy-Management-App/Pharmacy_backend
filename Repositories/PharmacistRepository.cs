using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
namespace PharmacyAPI.Repositories
{
    public class PharmacistRepository
    {
        private readonly AppDbContext _context;
        public PharmacistRepository(AppDbContext context) { _context = context; }
        public async Task<List<Pharmacist>> GetAllAsync()
        => await _context.Pharmacists
        .Include(p => p.User)
        .ToListAsync();
        public async Task<Pharmacist?> GetByIdAsync(int id)
        => await _context.Pharmacists
        .Include(p => p.User)
        .FirstOrDefaultAsync(p => p.Id == id);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
