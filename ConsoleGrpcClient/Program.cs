// See https://aka.ms/new-console-template for more information
using ConsoleGrpcClient;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args).Build();

var configuration = host.Services.GetService<IConfiguration>();
using var channel = GrpcChannel.ForAddress($"http://aspnetcoregrpcservice:{configuration["Grpc_ServicePort"]}");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Hello From ConsoleGrpcClient " });
Console.WriteLine($"收到服务端消息:{reply.Message}");

await host.RunAsync();

