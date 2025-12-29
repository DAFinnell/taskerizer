using System.Net;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Taskerizer.Tests;

public class TenancyTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public TenancyTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DiagnosticReturnsResolvedTenant()
    {
        var response = await _client.GetAsync("t/derekco/__diagnostic");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var body = await response.Content.ReadAsStringAsync();
        var json = JsonNode.Parse(body)!;

        bool isResolved = json["isResolved"]!.GetValue<bool>();
        string tenantSlug = json["tenantSlug"]!.GetValue<string>();

        Assert.True(isResolved);
        Assert.Equal("derekco", tenantSlug);
    }
}