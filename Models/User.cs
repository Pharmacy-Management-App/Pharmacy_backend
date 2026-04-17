using System;
using System.Collections.Generic;
namespace PharmacyAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer"; // Admin / Pharmacist / Customer
        public bool IsEmailVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();
        public ICollection<EmailVerification> EmailVerifications { get; set; } = new List<EmailVerification>();
        public Pharmacist? Pharmacist { get; set; }
    }
}
