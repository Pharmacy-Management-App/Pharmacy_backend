// DTOs/PrescriptionDTOs/PrescriptionDto.cs
namespace PharmacyAPI.DTOs.PrescriptionDTOs
{
    public class PrescriptionUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }

    public class PrescriptionReviewDto
    {
        public int PrescriptionId { get; set; }
        public string? Notes { get; set; }
    }

    public class PrescriptionResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ReviewedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }
}
