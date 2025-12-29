
using Microsoft.AspNetCore.Mvc;
using Taskerizer.Middleware.Tenancy;

namespace Taskerizer.Controllers;

[ApiController]
[Route("t/{tenantSlug}/__diagnostic")]
public class TenantDiagnosticController : ControllerBase
{
    private readonly ITenantContext _tenantContext;

    public TenantDiagnosticController(ITenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    [HttpGet]
    public ActionResult GetContext()
    {
        return Ok(new 
        {
            isResolved = _tenantContext.IsResolved,
            tenantSlug = _tenantContext.TenantSlug
        });
    }
}