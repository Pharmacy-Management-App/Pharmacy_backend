// Controllers/PrescriptionController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs.PrescriptionDTOs;
using PharmacyAPI.Services;
using System.Security.Claims;

namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly PrescriptionService _service;
        public PrescriptionController(PrescriptionService service) => _service = service;

        // POST /api/prescription/upload  (Customer)
        [HttpPost("upload")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Upload([FromForm] PrescriptionUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.UploadAsync(userId, dto.File);

            return Ok(result);
        }

        // GET /api/prescription/pending  (Pharmacist)
        [HttpGet("pending")]
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _service.GetPendingAsync();
            return Ok(list);
        }

        // GET /api/prescription/approved  (Pharmacist)
        [HttpGet("approved")]
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> GetApproved()
        {
            var list = await _service.GetApprovedAsync();
            return Ok(list);
        }

        // GET /api/prescription/my  (Customer)
        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMine()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var list = await _service.GetByUserIdAsync(userId);
            return Ok(list);
        }

        // PUT /api/prescription/approve  (Pharmacist)
        [HttpPut("approve")]
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> Approve(
            [FromBody] PrescriptionReviewDto dto)
        {
            var reviewerName = User.FindFirstValue(ClaimTypes.Name) ?? "Pharmacist";
            var ok = await _service.ApproveAsync(
                dto.PrescriptionId, reviewerName, dto.Notes);
            if (!ok) return NotFound(new { message = "Prescription not found." });
            return Ok(new { message = "Prescription approved." });
        }

        // PUT /api/prescription/reject  (Pharmacist)
        [HttpPut("reject")]
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> Reject(
            [FromBody] PrescriptionReviewDto dto)
        {
            var reviewerName = User.FindFirstValue(ClaimTypes.Name) ?? "Pharmacist";
            var ok = await _service.RejectAsync(
                dto.PrescriptionId, reviewerName, dto.Notes);
            if (!ok) return NotFound(new { message = "Prescription not found." });
            return Ok(new { message = "Prescription rejected." });
        }
    }
}
