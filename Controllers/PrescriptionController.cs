using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs;
using PharmacyAPI.Services;
namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/prescription")]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly PrescriptionService _service;
        public PrescriptionController(PrescriptionService service) { _service = service; }
        [HttpGet("pending")]
        [Authorize(Roles = "Pharmacist,Admin")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _service.GetByStatusAsync("Pending");
            return Ok(list);
        }
        [HttpGet("approved")]
        [Authorize(Roles = "Pharmacist,Admin")]
        public async Task<IActionResult> GetApproved()
        {
            var list = await _service.GetByStatusAsync("Approved");
            return Ok(list);
        }
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Pharmacist,Admin")]
        public async Task<IActionResult> Approve(int id, [FromBody] ReviewPrescriptionDto dto)
        {
            var reviewedBy = User.FindFirstValue("fullName") ?? "Pharmacist";
            var result = await _service.ReviewAsync(id, "approve", reviewedBy, dto.Notes);
            if (!result) return NotFound(new { message = "Prescription not found." });
            return Ok(new { message = "Prescription approved." });
        }
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Pharmacist,Admin")]
        public async Task<IActionResult> Reject(int id, [FromBody] ReviewPrescriptionDto dto)
        {
            var reviewedBy = User.FindFirstValue("fullName") ?? "Pharmacist";
            var result = await _service.ReviewAsync(id, "reject", reviewedBy, dto.Notes);
            if (!result) return NotFound(new { message = "Prescription not found." });
            return Ok(new { message = "Prescription rejected." });
        }
    }
}
