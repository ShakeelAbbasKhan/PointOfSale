using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PointOfSale.Data;
using System.Security;
using System.Security.Claims;

namespace PointOfSale.PermissionRelated
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        private readonly IPermissionService _permissionService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PermissionAuthorizationHandler(IPermissionService permissionService,UserManager<ApplicationUser> userManager)
        {

            _permissionService = permissionService;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var email = context.User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByEmailAsync(email);
            string? memberId = null;

            if (user != null)
            {
                memberId = user.Id;
            }

            if (memberId == null)
            {
                return;
            }

            HashSet<string> permissions = await _permissionService.GetPermissionsAsync(memberId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }


            //if (context.User == null)
            //{
            //    return;
            //}
            //var permissionss = context.User.Claims.Where(x => x.Type == "Permission" &&
            //                                                x.Value == requirement.Permission);
            //if (permissionss.Any())
            //{
            //    context.Succeed(requirement);
            //    return;
            //}
        }
    }
}
