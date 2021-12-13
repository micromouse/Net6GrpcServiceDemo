using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Net6GrpcServiceDemo.WorkerServiceGrpcService.GrpcServices;

namespace Net6GrpcServiceDemo.WorkerServiceGrpcService {
    /// <summary>
    /// 启动Grpc后台服务
    /// </summary>
    public class StartGrpcServiceWorker : BackgroundService {
        private readonly ILogger<StartGrpcServiceWorker> _logger;

        /// <summary>
        /// 初始化启动Grpc后台服务
        /// </summary>
        /// <param name="logger">日志器</param>
        public StartGrpcServiceWorker(ILogger<StartGrpcServiceWorker> logger) {
            _logger = logger;
        }

        /// <summary>
        /// 异步执行开启Grpc后台服务
        /// </summary>
        /// <param name="stoppingToken">取消Token</param>
        /// <returns>开启Grpc后台服务任务</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            var builder = WebApplication
                .CreateBuilder(new WebApplicationOptions {
                    ContentRootPath = Directory.GetCurrentDirectory(),
                    WebRootPath = Directory.GetCurrentDirectory()
                });
            builder.WebHost
                .ConfigureServices(services => services.AddGrpc())
                .ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(5001, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
                });

            var app = builder.Build();
            app.MapGrpcService<GreeterService>();
            app.Map("/status", app => app.Run(async context => await context.Response.WriteAsync("后台服务Grpc服务端正在运行中")));
            await app.RunAsync(stoppingToken);

            _logger.LogWarning("后台服务Grpc服务端已退出");
        }
    }
}