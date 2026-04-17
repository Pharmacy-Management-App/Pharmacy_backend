using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Services;
using PharmacyAPI.DTOs;
using System.Security.Claims;
namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/pharmacist")]
    [Authorize(Roles = "Pharmacist")]
    public class PharmacistController : ControllerBase
    {
        private readonly PrescriptionService _prescriptionService;
        public PharmacistController(PrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }
        [HttpGet("prescriptions/pending")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _prescriptionService.GetByStatusAsync("Pending");
            return Ok(list);
        }
        [HttpGet("prescriptions/approved")]
        public async Task<IActionResult> GetApproved()
        {
            var list = await _prescriptionService.GetByStatusAsync("Approved");
            return Ok(list);
        }
        [HttpPut("prescriptions/approve/{id}")]
        public async Task<IActionResult> Approve(int id, [FromBody] ReviewPrescriptionDto dto)
        {
            var reviewedBy = User.FindFirstValue("fullName") ?? "Pharmacist";
            var result = await _prescriptionService.ReviewAsync(id, "approve", reviewedBy, dto.Notes);
            if (!result) return NotFound(new { message = "Prescription not found." });
            return Ok(new { message = "Approved successfully." });
        }
        [HttpPut("prescriptions/reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] ReviewPrescriptionDto dto)
        {
            var reviewedBy = User.FindFirstValue("fullName") ?? "Pharmacist";
            var result = await _prescriptionService.ReviewAsync(id, "reject", reviewedBy, dto.Notes);
            if (!result) return NotFound(new { message = "Prescription not found." });
            return Ok(new { message = "Rejected successfully." });
        }
    }
}