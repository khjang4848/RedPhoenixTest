namespace RedPhoenix.Web.Filters;

using Microsoft.AspNetCore.Authorization;

public class Policies
{
    public const string Admin = "Admin";
    public const string User = "User";

    public static AuthorizationPolicy AdminPolicy()
        => new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            .RequireRole(Admin).Build();

    public static AuthorizationPolicy UserPolicy()
        => new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            .RequireRole(User).Build();
}

