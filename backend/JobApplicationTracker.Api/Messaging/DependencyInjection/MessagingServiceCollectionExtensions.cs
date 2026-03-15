using JobApplicationTracker.Api.Messaging.Configurations;
using JobApplicationTracker.Api.Messaging.Connection;
using JobApplicationTracker.Api.Messaging.Publishers;
using JobApplicationTracker.Api.Messaging.Serialization;
using System.Text.Json;

namespace JobApplicationTracker.Api.Messaging.DependencyInjection
{
    public static class MessagingServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqMessaging(
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
            services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();

            return services;
        }
    }
}
