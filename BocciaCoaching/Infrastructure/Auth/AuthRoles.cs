using BocciaCoaching.Utils;

namespace BocciaCoaching.Infrastructure.Auth;

/// <summary>
/// ES: Nombres de rol que viajan en el claim "role" del token y se usan en
/// <c>[Authorize(Roles = ...)]</c>. Se derivan de los GUID fijos de <see cref="RoleIds"/>.
/// </summary>
public static class AuthRoles
{
    public const string Admin = "Admin";
    public const string Coach = "Coach";
    public const string Athlete = "Athlete";

    public static string? FromRoleId(Guid roleId)
    {
        if (roleId == RoleIds.Admin) return Admin;
        if (roleId == RoleIds.Coach) return Coach;
        if (roleId == RoleIds.Athlete) return Athlete;
        return null;
    }

    /// <summary>ES: Roles que un usuario puede elegir al registrarse por sí mismo.</summary>
    public static bool IsSelfAssignable(Guid roleId) => roleId == RoleIds.Coach || roleId == RoleIds.Athlete;
}
