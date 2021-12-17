// See https://aka.ms/new-console-template for more information
using ConsoleGrpcClient;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args).Build();

TimingRequestSayHello();
await host.RunAsync();

/// <summary>
/// 定时请求<see cref="Greeter.GreeterClient.SayHelloAsync(HelloRequest, Grpc.Core.CallOptions)(HelloRequest)"/>
/// </summary>
async Task TimingRequestSayHello() {
    Thread.Sleep(10000);

    var configuration = host.Services.GetService<IConfiguration>();
    using var channel = GrpcChannel.ForAddress($"http://aspnetcoregrpcservice:{configuration["Grpc_ServicePort"]}");
    var client = new Greeter.GreeterClient(channel);
    var reply = await client.SayHelloAsync(new HelloRequest { Name = "Hello From ConsoleGrpcClient " });
    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:sss}]收到服务端消息:{reply.Message}");

    await TimingRequestSayHello();
}