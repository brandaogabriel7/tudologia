using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Lime.Messaging.Resources;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RestEase;

using Serilog;
using Serilog.Exceptions;

using Take.Api.Tudologia.Facades.Interfaces;
using Take.Api.Tudologia.Facades.Strategies.ExceptionHandlingStrategies;
using Take.Api.Tudologia.Models.UI;
using Take.Blip.Client;

using Constants = Take.Api.Tudologia.Models.Constants;

namespace Take.Api.Tudologia.Facades.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        private const string APPLICATION_KEY = "Application";
        private const string SETTINGS_SECTION = "Settings";

        /// <summary>
        /// Registers project's specific services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSingletons(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(SETTINGS_SECTION).Get<ApiSettings>();

            // Dependency injection
            services.AddSingleton(settings)
                    .AddSingleton(settings.BlipBotSettings)
                    .AddSingleton<IClassesFacade, ClassesFacade>()
                    .AddSingleton<IBlipFacade, BlipFacade>()
                    .AddSingleton(InstantiateBlipClient(settings.BlipBotSettings));

            // TODO: Criar blip client e injetar como Singleton.

            services.AddSingleton(provider =>
            {
                var logger = provider.GetService<ILogger>();
                return new Dictionary<Type, ExceptionHandlingStrategy>
                {
                    { typeof(ApiException), new ApiExceptionHandlingStrategy(logger) },
                    { typeof(NotImplementedException), new NotImplementedExceptionHandlingStrategy(logger) }
                };
            });

            // SERILOG settings
            services.AddSingleton<ILogger>(new LoggerConfiguration()
                     .ReadFrom.Configuration(configuration)
                     .Enrich.WithMachineName()
                     .Enrich.WithProperty(APPLICATION_KEY, Constants.PROJECT_NAME)
                     .Enrich.WithExceptionDetails()
                     .CreateLogger());
        }

        private static ISender InstantiateBlipClient(BlipBotSettings blipBotSettings)
        {
            return new BlipClientBuilder()
                .UsingAccessKey(blipBotSettings.Identifier, blipBotSettings.AccessKey)
                .UsingInstance($"{Constants.PROJECT_NAME} - {Environment.MachineName}")
                .UsingRoutingRule(RoutingRule.Instance)
                .Build();
        }
    }
}
