using Net6GrpcServiceDemo.WorkerServiceGrpcService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<StartGrpcServiceWorker>();
    })
    .Build();

await host.RunAsync();
