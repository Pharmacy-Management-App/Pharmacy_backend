using System;
namespace PharmacyAPI.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ReviewedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }
    public class ReviewPrescriptionDto
    {
        public string? Notes { get; set; }
    }
}