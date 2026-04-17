using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_backend.DTOs;
using Pharmacy_backend.Services;

namespace Pharmacy_backend.Controllers
{
    [ApiController]
    [Route("api/medicine")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _service;

        public MedicineController(IMedicineService service)
        {
            _service = service;
        }

        // ── GET /api/medicine ──────────────────────────────────
        // Public: all users can browse medicines
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] MedicineQueryParams query)
        {
            var result = await _service.GetAllMedicinesAsync(query);
            return Ok(result);
        }

        // ── GET /api/medicine/categories ───────────────────────
        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _service.GetCategoriesAsync();
            return Ok(categories);
        }

        // ── GET /api/medicine/{id} ─────────────────────────────
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var medicine = await _service.GetMedicineByIdAsync(id);
            if (medicine == null)
                return NotFound(new { message = $"Medicine with ID {id} not found." });

            return Ok(medicine);
        }

        // ── POST /api/medicine ─────────────────────────────────
        // Admin only
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMedicineDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateMedicineAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ── PUT /api/medicine/{id} ─────────────────────────────
        // Admin only
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicineDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateMedicineAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = $"Medicine with ID {id} not found." });

            return Ok(updated);
        }

        // ── DELETE /api/medicine/{id} ──────────────────────────
        // Admin only (soft delete)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteMedicineAsync(id);
            if (!success)
                return NotFound(new { message = $"Medicine with ID {id} not found." });

            return Ok(new { message = "Medicine deactivated successfully." });
        }
    }
}