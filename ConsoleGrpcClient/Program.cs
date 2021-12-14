// See https://aka.ms/new-console-template for more information
using ConsoleGrpcClient;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args).Build();

var httpClientHandler = new HttpClientHandler();
httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, httpClientHandler) {
    HttpVersion = new Version(1, 1)
};

var configuration = host.Services.GetService<IConfiguration>();
using var channel = GrpcChannel.ForAddress($"http://192.168.1.112:{configuration["Grpc_ServicePort"]}", new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Hello From ConsoleGrpcClient " });
Console.WriteLine($"Greeting:{reply.Message}");

await host.RunAsync();

