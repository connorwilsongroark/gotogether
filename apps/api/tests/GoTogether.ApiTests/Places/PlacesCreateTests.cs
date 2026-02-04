using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Xunit;
using GoTogether.Features.Places;
using GoTogether.Data;
using Microsoft.Extensions.DependencyInjection;

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
    public async Task POST_places_without_name_returns_400()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/v1/places",
            new { name = ""}
        );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
[Fact]
public async Task POST_places_returns_201_and_persists_place()
{
    var request = new
    {
        name = "Duke's House",
        description = "This is where Duke sleeps and eats"
    };

    var postResponse = await _client.PostAsJsonAsync("/api/v1/places", request);

    Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
    Assert.NotNull(postResponse.Headers.Location);

    var created = await postResponse.Content.ReadFromJsonAsync<CreatePlaceResponse>();
    Assert.NotNull(created);
    Assert.NotEqual(Guid.Empty, created!.Id);
    Assert.Equal("Duke's House", created.Name);

        // Then – persistence
    using var scope = _factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var placeInDb = await db.Places.FindAsync(created.Id);
    Assert.NotNull(placeInDb);
    Assert.Equal("Duke's House", placeInDb!.Name);
    Assert.Equal("This is where Duke sleeps and eats", placeInDb.Description);
    Assert.Equal("auth0|test-user-1", placeInDb.OwnerUserId);
}
}