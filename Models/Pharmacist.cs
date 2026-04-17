using System;

namespace PharmacyAPI.Models
{
    public class Pharmacist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected
        public string? ProfileImagePath { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedAt { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;
    }
}
