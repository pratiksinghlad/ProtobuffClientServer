using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Net9Solution.Api.Models;

var services = new ServiceCollection();
services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5111");
});

var serviceProvider = services.BuildServiceProvider();
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

var apiClient = httpClientFactory.CreateClient("ApiClient");

// Request Protobuf
var requestProtobuf = new HttpRequestMessage(HttpMethod.Get, "/data");
requestProtobuf.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
var responseProtobuf = await apiClient.SendAsync(requestProtobuf);
var complexDataProtobuf = ComplexData.Parser.ParseFrom(await responseProtobuf.Content.ReadAsByteArrayAsync());
Console.WriteLine("Protobuf Response:");
Console.WriteLine(complexDataProtobuf);

// Request JSON
var requestJson = new HttpRequestMessage(HttpMethod.Get, "/data");
requestJson.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
var responseJson = await apiClient.SendAsync(requestJson);
var complexDataJson = await responseJson.Content.ReadFromJsonAsync<ComplexData>();
Console.WriteLine("JSON Response:");
Console.WriteLine(complexDataJson);
