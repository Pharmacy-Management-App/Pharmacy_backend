using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.DTOs;
using PharmacyAPI.Repositories;
namespace PharmacyAPI.Services
{
    public class PrescriptionService
    {
        private readonly PrescriptionRepository _repo;
        public PrescriptionService(PrescriptionRepository repo) { _repo = repo; }
        public async Task<List<PrescriptionDto>> GetByStatusAsync(string status)
        {
            var list = await _repo.GetByStatusAsync(status);
            return list.Select(p => new PrescriptionDto
            {
                Id = p.Id,
                UserId = p.UserId,
                UserName = p.User?.FullName ?? "",
                FilePath = p.FilePath,
                OriginalFileName = p.OriginalFileName,
                Status = p.Status,
                ReviewedBy = p.ReviewedBy,
                Notes = p.Notes,
                UploadedAt = p.UploadedAt,
                ReviewedAt = p.ReviewedAt
            }).ToList();
        }
        public async Task<bool> ReviewAsync(int id, string action, string reviewedBy, string? notes)
        {
            var prescription = await _repo.GetByIdAsync(id);
            if (prescription == null) return false;
            prescription.Status = action == "approve" ? "Approved" : "Rejected";
            prescription.ReviewedBy = reviewedBy;
            prescription.Notes = notes;
            prescription.ReviewedAt = DateTime.UtcNow;
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}