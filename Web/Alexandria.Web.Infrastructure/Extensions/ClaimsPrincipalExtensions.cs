namespace Alexandria.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    using Alexandria.Common;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static bool IsAdministrator(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.IsInRole(GlobalConstants.AdministratorRoleName);
    }
}
