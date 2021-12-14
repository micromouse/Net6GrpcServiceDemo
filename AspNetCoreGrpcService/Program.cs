using Microsoft.AspNetCore.Server.Kestrel.Core;
using Net6GrpcServiceDemo.AspNetCoreGrpcService.Services;

var builder = WebApplication
    .CreateBuilder(args);
builder.WebHost
    .ConfigureKestrel(x =>
    {
        x.ListenAnyIP(80, x1 => x1.Protocols = HttpProtocols.Http1AndHttp2);
        x.ListenAnyIP(443, x1 => x1.Protocols = HttpProtocols.Http2);
    })
    .ConfigureServices(x =>
    {
        x.AddGrpc();
    });

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
