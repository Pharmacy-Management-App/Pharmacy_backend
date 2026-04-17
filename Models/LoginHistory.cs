using System;

namespace PharmacyAPI.Models
{
    public class LoginHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string DeviceInfo { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public DateTime LoginAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;
    }
}
