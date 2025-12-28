namespace Taskerizer.Middleware.Tenancy;

public class RouteTenantResolver : ITenantResolver 
{
    private const string RouteKey = "tenantSlug";
    public RouteTenantResolver() {}

    public TenantResolutionResult Resolve(HttpContext httpContext)
    {
        if (!httpContext.Request.RouteValues.TryGetValue(RouteKey, out var value))
        {
            return TenantResolutionResult.NotFound();
        }

        if (value is not string slug)
        {
            return TenantResolutionResult.NotFound();
        }
        return TenantResolutionResult.FoundTenant(slug);
    }
}