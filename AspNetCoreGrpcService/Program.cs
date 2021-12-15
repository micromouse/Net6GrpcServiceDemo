using Microsoft.AspNetCore.Server.Kestrel.Core;
using Net6GrpcServiceDemo.AspNetCoreGrpcService.Services;
using System.Text.Encodings.Web;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost
    .ConfigureLogging(logging =>
    {
        logging
            .AddJsonConsole(options =>
            {
                options.IncludeScopes = false;
                options.TimestampFormat = "hh:mm:sss";
                options.JsonWriterOptions = new JsonWriterOptions {
                    Indented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
            });
    })
    .ConfigureKestrel(x =>
    {
        x.ListenAnyIP(80, x1 => x1.Protocols = HttpProtocols.Http1AndHttp2);
        x.ListenAnyIP(81, x1 => x1.Protocols = HttpProtocols.Http2);
    });
builder.Services.AddGrpc();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app
    .MapGet("/", () =>
    {
        app.Services.GetService<ILogger<GreeterService>>()?.LogInformation("这是中文");
        return "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909";
    });

app.Run();
