namespace Taskerizer.Middleware.Tenancy;
public interface ITenantContext
{
    string TenantSlug {get; }
    bool IsResolved {get; }
}