# Protocol Buffer Client-Server Example

This project demonstrates how to implement Protocol Buffers (protobuf) in a .NET client-server application. It includes both protobuf and JSON serialization for comparison.

## Project Structure

- `Net9Solution.Api`: ASP.NET Core Web API that serves both protobuf and JSON responses
- `Net9Solution.Client`: Console application client that demonstrates consuming the API
- `Net9Solution.Api.Tests`: Unit tests for the API endpoints
- `Net9Solution.Client.Tests`: Unit tests for the client functionality

## Features

- Protocol Buffer serialization/deserialization
- JSON serialization/deserialization
- Custom Protocol Buffer output formatter
- Complex data structure support
- Comprehensive unit tests

## Prerequisites

- .NET 9.0 SDK
- Protocol Buffer compiler (protoc)

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the API:

   ```bash
   cd Net9Solution.Api
   dotnet run
   ```

4. In a separate terminal, run the client:

   ```bash
   cd Net9Solution.Client
   dotnet run
   ```

## API Endpoints

### GET /data

Returns a complex data structure in either protobuf or JSON format based on the Accept header:

- `application/x-protobuf` for Protocol Buffer format
- `application/json` for JSON format

## Data Structure

The project uses the following Protocol Buffer message definition:

```protobuf
message ComplexData {
    string string_value = 1;
    int32 int32_value = 2;
    bool bool_value = 3;
    repeated string string_array = 4;
    map<string, string> string_map = 5;
    NestedData nested_data = 6;
}

message NestedData {
    string nested_string_value = 1;
    double nested_double_value = 2;
}
```

## Running Tests

To run the tests:

```bash
dotnet test
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
