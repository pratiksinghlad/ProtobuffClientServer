using System.Runtime.CompilerServices;
using Google.Protobuf.Collections;
using Net9Solution.Api.Formatters;
using Net9Solution.Api.Models;

[assembly: InternalsVisibleTo("Net9Solution.Api.Tests")]

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvcCore(options =>
{
    options.OutputFormatters.Add(new ProtobufOutputFormatter());
});

var app = builder.Build();

app.MapGet("/data", () =>
{
    return new ComplexData
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
});

app.Run();

public partial class Program { }
