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

    [Theory]
    [InlineData("derekco")]
    [InlineData("DeReKcO")]
    [InlineData("%20DeReKcO%20")]
    public async Task DiagnosticReturnsResolvedTenant(string slug)
    {
        var response = await _client.GetAsync($"/t/{slug}/__diagnostic");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var body = await response.Content.ReadAsStringAsync();
        var json = JsonNode.Parse(body)!;

        bool isResolved = json["isResolved"]!.GetValue<bool>();
        string tenantSlug = json["tenantSlug"]!.GetValue<string>();
        var expected = Uri.UnescapeDataString(slug)
                  .Trim()
                  .ToLowerInvariant();

        Assert.True(isResolved);
        Assert.Equal(expected, tenantSlug);
    }

    [Theory]
    [InlineData("%20")]
    public async Task CheckInvalidTenantSlug(string slug)
    {
        var response = await _client.GetAsync($"/t/{slug}/__diagnostic");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json", response.Content.Headers.ContentType!.MediaType);

        var body = await response.Content.ReadAsStringAsync();
        var json = JsonNode.Parse(body)!;

        string title = json["title"]!.GetValue<string>();
        string detail = json["detail"]!.GetValue<string>();
        int status = json["status"]!.GetValue<int>();

        Assert.Equal("Invalid tenant slug", title);
        Assert.Equal(400, status);
        Assert.Contains("malformed", detail);
    }
}