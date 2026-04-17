using System;
using System.Collections.Generic;

namespace PharmacyAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? PrescriptionId { get; set; }
        public string Status { get; set; } = "Pending"; // Pending / Processing / Shipped / Delivered / Cancelled
        public decimal TotalAmount { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty; // COD / Online
        public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid / Paid / Refunded
        public string? Notes { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;
        public Prescription? Prescription { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
