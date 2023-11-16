# gRpc .Net Core 

Welcome to the gRPC .NET Sample Repository! This repository provides a basic setup for building gRPC client and server projects using .NET.

## Getting Started
### Prerequisites
Make sure you have the following installed on your machine:

.NET SDK
Visual Studio, Rider or Visual Studio Code (optional but recommended)

# Sample Code
gRPC Server
Check out the GreeterService class in the Server project for a basic gRPC service implementation.
```
public class GreeterService : Greeter.GreeterBase
{
public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
{
return Task.FromResult(new HelloReply
{
Message = $"Hello, {request.Name}!"
});
}
}

```
#gRPC Client

```
var channel = new Channel("localhost:50051", ChannelCredentials.Insecure);

var client = new Greeter.GreeterClient(channel);

var request = new HelloRequest { Name = "World" };

var response = client.SayHello(request);

Console.WriteLine($"Greeting: {response.Message}");

```
Feel free to modify and extend these samples to fit your specific use case.
# License
This project is licensed under the MIT License.