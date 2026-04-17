using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Models;
using PharmacyAPI.Services;
namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminService) { _adminService = adminService; }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("pharmacists")]
        public async Task<IActionResult> GetPharmacists()
        {
            var pharmacists = await _adminService.GetAllPharmacistsAsync();
            return Ok(pharmacists);
        }
        [HttpPut("pharmacist/approve/{id}")]
        public async Task<IActionResult> ApprovePharmacist(int id)
        {
            var result = await _adminService.UpdatePharmacistStatusAsync(id, "Approved");
            if (!result) return NotFound(new { message = "Pharmacist not found." });
            return Ok(new { message = "Pharmacist approved." });
        }
        [HttpPut("pharmacist/reject/{id}")]
        public async Task<IActionResult> RejectPharmacist(int id)
        {
            var result = await _adminService.UpdatePharmacistStatusAsync(id, "Rejected");
            if (!result) return NotFound(new { message = "Pharmacist not found." });
            return Ok(new { message = "Pharmacist rejected." });
        }
        [HttpGet("medicines")]
        public async Task<IActionResult> GetMedicines()
        {
            var medicines = await _adminService.GetAllMedicinesAsync();
            return Ok(medicines);
        }
        [HttpPut("medicines/{id}")]
        public async Task<IActionResult> UpdateMedicine(int id, [FromBody] Medicine medicine)
        {
            var result = await _adminService.UpdateMedicineAsync(id, medicine);
            if (!result) return NotFound(new { message = "Medicine not found." });
            return Ok(new { message = "Medicine updated." });
        }
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventory()
        {
            var inventory = await _adminService.GetAllInventoryAsync();
            return Ok(inventory);
        }
        [HttpPut("inventory/{medicineId}")]
        public async Task<IActionResult> UpdateStock(int medicineId, [FromBody] UpdateStockDto dto)
        {
            var result = await _adminService.UpdateStockAsync(medicineId, dto.Quantity);
            if (!result) return NotFound(new { message = "Inventory not found." });
            return Ok(new { message = "Stock updated." });
        }
    }
    public class UpdateStockDto { public int Quantity { get; set; } }
}