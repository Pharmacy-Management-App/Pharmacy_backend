using System;

namespace PharmacyAPI.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int QuantityInStock { get; set; } = 0;
        public int ReorderLevel { get; set; } = 10;
        public int MaxStockLevel { get; set; } = 500;
        public DateTime? ExpiryDate { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Medicine Medicine { get; set; } = null!;
    }
}
