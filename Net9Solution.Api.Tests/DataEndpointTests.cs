using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Net9Solution.Api.Models;
using Xunit;
using System.Net.Http.Json;


namespace Net9Solution.Api.Tests;

public class DataEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DataEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetData_ReturnsProtobuf()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/data");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

        // Act
        var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var data = ComplexData.Parser.ParseFrom(await response.Content.ReadAsByteArrayAsync());
        Assert.Equal("Hello, World!", data.StringValue);
    }

    [Fact]
    public async Task GetData_ReturnsJson()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/data");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Act
        var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<ComplexData>();
        Assert.Equal("Hello, World!", data.StringValue);
    }
}
