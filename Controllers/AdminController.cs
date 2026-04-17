// Controllers/AdminController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Models;
using PharmacyAPI.Services;

namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _service;
        public AdminController(AdminService service) => _service = service;

        // GET /api/admin/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var stats = await _service.GetDashboardStatsAsync();
            return Ok(stats);
        }

        // ── Users ──────────────────────────────────────────────────────────
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var ok = await _service.ToggleUserStatusAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "User status updated." });
        }

        // ── Pharmacists ────────────────────────────────────────────────────
        [HttpGet("pharmacists")]
        public async Task<IActionResult> GetPharmacists()
        {
            var list = await _service.GetAllPharmacistsAsync();
            return Ok(list);
        }

        [HttpGet("pharmacists/pending")]
        public async Task<IActionResult> GetPendingPharmacists()
        {
            var list = await _service.GetPendingPharmacistsAsync();
            return Ok(list);
        }

        [HttpPut("pharmacists/{id}/approve")]
        public async Task<IActionResult> ApprovePharmacist(int id)
        {
            var ok = await _service.ApprovePharmacistAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Pharmacist approved." });
        }

        [HttpPut("pharmacists/{id}/reject")]
        public async Task<IActionResult> RejectPharmacist(int id)
        {
            var ok = await _service.RejectPharmacistAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Pharmacist rejected." });
        }

        // ── Medicines ──────────────────────────────────────────────────────
        [HttpGet("medicines")]
        public async Task<IActionResult> GetMedicines()
        {
            var list = await _service.GetAllMedicinesAsync();
            return Ok(list);
        }

        [HttpPost("medicines")]
        public async Task<IActionResult> AddMedicine([FromBody] Medicine medicine)
        {
            var result = await _service.AddMedicineAsync(medicine);
            return CreatedAtAction(nameof(GetMedicines), result);
        }

        [HttpPut("medicines/{id}/toggle-status")]
        public async Task<IActionResult> ToggleMedicineStatus(int id)
        {
            var medicine = new Medicine { Id = id };
            var ok = await _service.ToggleMedicineStatusAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Medicine status updated." });
        }

        // ── Inventory ──────────────────────────────────────────────────────
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventory()
        {
            var inv = await _service.GetInventoryAsync();
            return Ok(inv);
        }

        [HttpPut("inventory/{medicineId}")]
        public async Task<IActionResult> UpdateInventory(
            int medicineId, [FromBody] int quantity)
        {
            var ok = await _service.UpdateInventoryAsync(medicineId, quantity);
            if (!ok) return NotFound();
            return Ok(new { message = "Inventory updated." });
        }
    }
}
