using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GoTogether.ApiTests.Places;

public class PlacesCreateTests : IClassFixture<TestAppFactory>
{
    private readonly TestAppFactory _factory;
    private readonly HttpClient _client;

    public PlacesCreateTests(TestAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_places_requires_authentication()
    {
        // Create a client WITHOUT test auth
        var unauthenticatedClient = _factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        var response = await unauthenticatedClient.PostAsJsonAsync(
            "/api/v1/places",
            new { name = "Test" }
        );

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    [Fact]
    public async Task POST_places_without_name_returns_400()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/v1/places",
            new { name = ""}
        );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}