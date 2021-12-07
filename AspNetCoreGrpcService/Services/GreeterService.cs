using AspNetCoreGrpcService;
using Grpc.Core;

namespace Net6GrpcServiceDemo.AspNetCoreGrpcService.Services {
    public class GreeterService : Greeter.GreeterBase {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger) {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            _logger.LogInformation("正在执行请求:[{@HelloRequest}]", request);
            return Task.FromResult(new HelloReply {
                Message = "Hello " + request.Name
            });
        }
    }
}