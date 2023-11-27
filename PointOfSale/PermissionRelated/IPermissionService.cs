namespace PointOfSale.PermissionRelated
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(string memberId);
    }
}
