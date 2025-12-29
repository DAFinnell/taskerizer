namespace Taskerizer.Middleware.Tenancy;

public class TenantResolutionResult
{
    public bool Found {get; }
    public string? TenantSlug {get; }

    private TenantResolutionResult(bool found, string? tenantSlug) 
    {
        Found = found;
        TenantSlug = tenantSlug;
    }

    public static TenantResolutionResult NotFound() => new TenantResolutionResult(false, null);
    public static TenantResolutionResult FoundTenant(string tenantSlug) => new TenantResolutionResult(true, tenantSlug);
}