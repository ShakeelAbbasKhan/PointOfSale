using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Data;
using PointOfSale.ViewModels;
using System.Security.Claims;

namespace PointOfSale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpGet("{roleId}")]
        public async Task<ActionResult<PermissionViewModel>> Index(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound();
            }

            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in claims)
            {
                var roleClaim = new RoleClaimsViewModel
                {
                    Type = claim.Type,
                    Value = claim.Value,
                    Selected = true,

                };

                allPermissions.Add(roleClaim);
            }

            model.RoleClaims = allPermissions;
            return Ok(model);
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);

            // Remove existing claims
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            // Add selected claims
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                var allClaims = await _roleManager.GetClaimsAsync(role);

                // Add the claim only if it doesn't exist
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == claim.Value))
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.Value));
                }
            }

            return Ok(new { Message = "Permissions updated successfully." });
        }

    }

}
