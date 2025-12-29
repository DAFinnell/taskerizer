namespace Taskerizer.Middleware.Tenancy;

public interface ITenantResolver
{
    TenantResolutionResult Resolve(HttpContext httpContext);
}