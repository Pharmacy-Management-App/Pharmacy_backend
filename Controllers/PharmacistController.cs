// Controllers/PharmacistController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Services;
using System.Security.Claims;

namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Pharmacist")]
    public class PharmacistController : ControllerBase
    {
        private readonly PrescriptionService _prescriptionService;
        private readonly AdminService _adminService;

        public PharmacistController(
            PrescriptionService prescriptionService,
            AdminService adminService)
        {
            _prescriptionService = prescriptionService;
            _adminService = adminService;
        }

        // GET /api/pharmacist/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var pending = await _prescriptionService.GetPendingAsync();
            var approved = await _prescriptionService.GetApprovedAsync();
            return Ok(new
            {
                PendingCount = pending.Count,
                ApprovedCount = approved.Count,
                RecentPending = pending.Take(5)
            });
        }
    }
}
