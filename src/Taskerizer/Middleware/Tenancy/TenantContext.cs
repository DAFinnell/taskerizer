namespace Taskerizer.Middleware.Tenancy;

public class TenantContext : ITenantContext {
    public string TenantSlug {get; private set; } = "";
    public bool IsResolved {get; private set;} = false;
    public TenantContext() { }

    internal void Resolve(string tenantSlug)
    {
        if (string.IsNullOrEmpty(tenantSlug))
        {
            throw new ArgumentException();
        }
        if (IsResolved == true)
        {
            throw new InvalidOperationException();
        }
        TenantSlug = tenantSlug.Trim().ToLower();
        IsResolved = true;
    }
}