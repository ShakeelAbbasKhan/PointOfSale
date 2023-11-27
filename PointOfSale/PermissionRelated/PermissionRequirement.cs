using Microsoft.AspNetCore.Authorization;

namespace PointOfSale.PermissionRelated
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
