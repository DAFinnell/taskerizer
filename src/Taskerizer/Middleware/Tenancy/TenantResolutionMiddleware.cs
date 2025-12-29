using Microsoft.AspNetCore.Mvc;

namespace Taskerizer.Middleware.Tenancy;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private const string TenantSlugKey = "tenantSlug";
    
    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext,
        ITenantResolver tenantResolver,
        ITenantContext tenantContext)
    {
        try
        {
            bool tenantRequired = httpContext.Request.RouteValues.ContainsKey(TenantSlugKey);
            TenantResolutionResult result = tenantResolver.Resolve(httpContext);

            if (tenantRequired && !result.Found)
            {
                await WriteBadRequest(
                    httpContext,
                    "Tenant required",
                    "Tenant is required to access this route");
                return;
            } 
            else if (tenantRequired && result.Found)
            {
                ((TenantContext)tenantContext).Resolve(result.TenantSlug!);
            }
            await _next(httpContext);
        } 
        catch (ArgumentException)
        {
            await WriteBadRequest(
                httpContext,
                "Invalid tenant slug",
                "The provided tenant identifier is malformed or invalid");
            return;
        } 
        catch (InvalidOperationException)
        {
            await WriteBadRequest(
                httpContext,
                "Tenant resolution error",
                "Tenant context cannot be resolved more than once");
            return;
        }
    }

    private static async Task WriteBadRequest(
        HttpContext httpContext,
        string title,
        string detail) 
    {
        var problem = new ProblemDetails
    {
        Status = StatusCodes.Status400BadRequest,
        Title = title,
        Detail = detail,
        Type = null
    };

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    httpContext.Response.ContentType = "application/problem+json";

    await httpContext.Response.WriteAsJsonAsync(problem);
    }
}