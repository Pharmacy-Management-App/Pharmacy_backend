using System;
using System.Collections.Generic;

namespace PharmacyAPI.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FilePath { get; set; } = string.Empty; // e.g., /uploads/prescriptions/file.jpg
        public string OriginalFileName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected
        public string? ReviewedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
