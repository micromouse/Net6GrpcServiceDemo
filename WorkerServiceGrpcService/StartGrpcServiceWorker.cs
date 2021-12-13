using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Net6GrpcServiceDemo.WorkerServiceGrpcService.GrpcServices;

namespace Net6GrpcServiceDemo.WorkerServiceGrpcService {
    /// <summary>
    /// ����Grpc��̨����
    /// </summary>
    public class StartGrpcServiceWorker : BackgroundService {
        private readonly ILogger<StartGrpcServiceWorker> _logger;

        /// <summary>
        /// ��ʼ������Grpc��̨����
        /// </summary>
        /// <param name="logger">��־��</param>
        public StartGrpcServiceWorker(ILogger<StartGrpcServiceWorker> logger) {
            _logger = logger;
        }

        /// <summary>
        /// �첽ִ�п���Grpc��̨����
        /// </summary>
        /// <param name="stoppingToken">ȡ��Token</param>
        /// <returns>����Grpc��̨��������</returns>
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
            app.Map("/status", app => app.Run(async context => await context.Response.WriteAsync("��̨����Grpc���������������")));
            await app.RunAsync(stoppingToken);

            _logger.LogWarning("��̨����Grpc��������˳�");
        }
    }
}