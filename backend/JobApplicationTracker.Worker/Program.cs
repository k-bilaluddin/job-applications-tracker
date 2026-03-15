using JobApplicationTracker.Worker.Messaging.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRabbitMqConsumers(builder.Configuration);

var host = builder.Build();
host.Run();