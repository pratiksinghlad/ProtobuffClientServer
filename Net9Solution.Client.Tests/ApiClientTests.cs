using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Net9Solution.Api.Models;
using Xunit;

namespace Net9Solution.Client.Tests;

public class ApiClientTests
{
    [Fact]
    public async Task Can_Handle_Protobuf_And_Json_Responses()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var complexData = new ComplexData
        {
            StringValue = "Hello, World!",
            Int32Value = 42,
            BoolValue = true,
        };
        complexData.StringArray.Add("one");
        complexData.StringArray.Add("two");
        complexData.StringMap.Add("key1", "value1");
        complexData.NestedData = new NestedData { NestedStringValue = "nested" };


        var protobufStream = new MemoryStream(complexData.ToByteArray());
        var jsonStream = new MemoryStream();
        await System.Text.Json.JsonSerializer.SerializeAsync(jsonStream, complexData);
        jsonStream.Position = 0;

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Headers.Accept.ToString() == "application/x-protobuf"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(protobufStream),
            });

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Headers.Accept.ToString() == "application/json"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(jsonStream),
            });

        var services = new ServiceCollection();
        services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri("http://localhost");
        }).ConfigurePrimaryHttpMessageHandler(() => mockHttpMessageHandler.Object);
        var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var apiClient = httpClientFactory.CreateClient("ApiClient");

        // Act & Assert
        // Protobuf
        var requestProtobuf = new HttpRequestMessage(HttpMethod.Get, "/data");
        requestProtobuf.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-protobuf"));
        var responseProtobuf = await apiClient.SendAsync(requestProtobuf);
        var complexDataProtobuf = ComplexData.Parser.ParseFrom(await responseProtobuf.Content.ReadAsByteArrayAsync());
        Assert.Equal("Hello, World!", complexDataProtobuf.StringValue);

        // JSON
        var requestJson = new HttpRequestMessage(HttpMethod.Get, "/data");
        requestJson.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        var responseJson = await apiClient.SendAsync(requestJson);
        var complexDataJson = await responseJson.Content.ReadFromJsonAsync<ComplexData>();
        Assert.Equal("Hello, World!", complexDataJson.StringValue);
    }
}
