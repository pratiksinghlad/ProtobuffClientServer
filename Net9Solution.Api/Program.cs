using System.Runtime.CompilerServices;
using Net9Solution.Api.Models;

[assembly: InternalsVisibleTo("Net9Solution.Api.Tests")]

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapGet("/data", (HttpContext context) =>
{
    var data = new ComplexData
    {
        StringValue = "Hello, World!",
        Int32Value = 42,
        BoolValue = true,
        StringArray = { "one", "two", "three" },
        StringMap = { { "key1", "value1" }, { "key2", "value2" } },
        NestedData = new NestedData
        {
            NestedStringValue = "nested string",
            NestedDoubleValue = 3.14
        }
    };

    var accept = context.Request.Headers.Accept.ToString();
    if (accept.Contains("application/x-protobuf"))
    {
        using var ms = new MemoryStream();
        using var output = new Google.Protobuf.CodedOutputStream(ms);
        data.WriteTo(output);
        output.Flush();
        var buffer = ms.ToArray();
        return Results.File(buffer, "application/x-protobuf");
    }

    var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions
    {
        PropertyNamingPolicy = null
    });
    return Results.Text(json, "application/json");
});

await app.RunAsync();

public partial class Program { }
