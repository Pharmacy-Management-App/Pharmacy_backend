using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
namespace PharmacyAPI.Repositories
{
    public class PrescriptionRepository
    {
        private readonly AppDbContext _context;
        public PrescriptionRepository(AppDbContext context) { _context = context; }
        public async Task<List<Prescription>> GetByStatusAsync(string status)
        => await _context.Prescriptions
        .Include(p => p.User)
        .Where(p => p.Status == status)
        .OrderByDescending(p => p.UploadedAt)
        .ToListAsync();
        public async Task<Prescription?> GetByIdAsync(int id)
        => await _context.Prescriptions
        .Include(p => p.User)
        .FirstOrDefaultAsync(p => p.Id == id);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}