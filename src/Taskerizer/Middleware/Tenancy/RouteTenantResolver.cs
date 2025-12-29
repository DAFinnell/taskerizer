namespace Taskerizer.Middleware.Tenancy;

public class RouteTenantResolver : ITenantResolver 
{
    private const string TenantSlugKey = "tenantSlug";
    public RouteTenantResolver() {}

    public TenantResolutionResult Resolve(HttpContext httpContext)
    {
        if (!httpContext.Request.RouteValues.TryGetValue(TenantSlugKey, out var value))
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