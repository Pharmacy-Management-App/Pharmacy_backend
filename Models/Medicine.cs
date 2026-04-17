using System;
using System.Collections.Generic;

namespace PharmacyAPI.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string DosageForm { get; set; } = string.Empty; // Tablet / Syrup / Capsule / Injection
        public string Strength { get; set; } = string.Empty;  // e.g., 500mg
        public bool RequiresPrescription { get; set; } = false;
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public Category Category { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Inventory? Inventory { get; set; }
    }
}
