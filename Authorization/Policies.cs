using Microsoft.AspNetCore.Authorization;

namespace SafeVault.Authorization
{
    public static class Policies
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";

        public static void ConfigurePolicies(AuthorizationOptions options)
        {
            options.AddPolicy(Admin, policy => 
                policy.RequireRole("Admin"));

            options.AddPolicy(Manager, policy => 
                policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") || 
                    context.User.IsInRole("Manager")));

            options.AddPolicy(User, policy => 
                policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") || 
                    context.User.IsInRole("Manager") || 
                    context.User.IsInRole("User")));
        }
    }
}