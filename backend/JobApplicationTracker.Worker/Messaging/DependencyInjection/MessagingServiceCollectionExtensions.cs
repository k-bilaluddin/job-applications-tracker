using JobApplicationTracker.Contracts.Messaging.Events;
using JobApplicationTracker.Worker.Messaging.Configurations;
using JobApplicationTracker.Worker.Messaging.Connection;
using JobApplicationTracker.Worker.Messaging.Consumers;
using JobApplicationTracker.Worker.Messaging.Handlers;
using JobApplicationTracker.Worker.Messaging.Interfaces;
using JobApplicationTracker.Worker.Messaging.Serialization;
using System.Text.Json;

namespace JobApplicationTracker.Worker.Messaging.DependencyInjection
{
    public static class MessagingServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqConsumers(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var rabbitMqOptions = configuration
                .GetSection(RabbitMqOptions.SectionName)
                .Get<RabbitMqOptions>()
                ?? throw new InvalidOperationException("RabbitMq configuration is missing.");

            services.AddSingleton(rabbitMqOptions);

            services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            services.AddSingleton<IEventSerializer, JsonEventSerializer>();
            services.AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>();

            services.AddSingleton<IMessageHandler<JobApplicationCreatedEvent>, JobApplicationCreatedEventHandler>();
            services.AddSingleton<IMessageConsumer, JobApplicationCreatedConsumer>();

            services.AddHostedService<MessagingHostedService>();

            return services;
        }
    }
}