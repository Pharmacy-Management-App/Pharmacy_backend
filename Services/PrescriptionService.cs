// Services/PrescriptionService.cs
using PharmacyAPI.DTOs.PrescriptionDTOs;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories;

namespace PharmacyAPI.Services
{
    public class PrescriptionService
    {
        private readonly PrescriptionRepository _repo;
        private readonly IWebHostEnvironment _env;

        public PrescriptionService(PrescriptionRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<PrescriptionResponseDto> UploadAsync(
            int userId, IFormFile file)
        {
            var uploadsPath = Path.Combine(_env.ContentRootPath,
                "uploads", "prescriptions");
            Directory.CreateDirectory(uploadsPath);

            var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsPath, uniqueName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var prescription = new Prescription
            {
                UserId = userId,
                FilePath = $"/uploads/prescriptions/{uniqueName}",
                OriginalFileName = file.FileName,
                Status = "Pending",
                UploadedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(prescription);
            return MapToDto(prescription);
        }

        public async Task<List<PrescriptionResponseDto>> GetPendingAsync()
        {
            var list = await _repo.GetPendingAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<List<PrescriptionResponseDto>> GetApprovedAsync()
        {
            var list = await _repo.GetApprovedAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<List<PrescriptionResponseDto>> GetByUserIdAsync(int userId)
        {
            var list = await _repo.GetByUserIdAsync(userId);
            return list.Select(MapToDto).ToList();
        }

        public async Task<bool> ApproveAsync(
            int prescriptionId, string reviewerName, string? notes)
        {
            var p = await _repo.GetByIdAsync(prescriptionId);
            if (p == null) return false;
            p.Status = "Approved";
            p.ReviewedBy = reviewerName;
            p.Notes = notes;
            p.ReviewedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(p);
            return true;
        }

        public async Task<bool> RejectAsync(
            int prescriptionId, string reviewerName, string? notes)
        {
            var p = await _repo.GetByIdAsync(prescriptionId);
            if (p == null) return false;
            p.Status = "Rejected";
            p.ReviewedBy = reviewerName;
            p.Notes = notes;
            p.ReviewedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(p);
            return true;
        }

        private static PrescriptionResponseDto MapToDto(Prescription p) => new()
        {
            Id = p.Id,
            UserId = p.UserId,
            FilePath = p.FilePath,
            OriginalFileName = p.OriginalFileName,
            Status = p.Status,
            ReviewedBy = p.ReviewedBy,
            Notes = p.Notes,
            UploadedAt = p.UploadedAt,
            ReviewedAt = p.ReviewedAt
        };
    }
}
