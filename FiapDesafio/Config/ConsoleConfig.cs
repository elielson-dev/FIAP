using FiapDesafio.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace FiapDesafio.Config
{
    public static class ConsoleConfig
    {
        public static void AddConsoleConfiguration(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            AddLogging(services);
            
            services.Configure<FiapApiSettings>(configuration.GetSection(nameof(FiapApiSettings)));

            services.AddSingleton<IFiapService, FiapService>();
            services.AddSingleton<IConsultarService, ConsultarService>();
            
            var uri = configuration.GetValue("FiapApiSettings:BaseURL", "text/plain");
            services.AddHttpClient<IFiapService, FiapService>(s => s.BaseAddress = new Uri(uri));
            
        }

        public static void AddLogging(IServiceCollection services)
        {
            string strExeFilePath = Assembly.GetExecutingAssembly().Location;
            string strWorkPath = Path.GetDirectoryName(strExeFilePath) ?? throw new ArgumentException(nameof(strExeFilePath));

            services.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .MinimumLevel.Override("System", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.File(path: @$"{strWorkPath}\logs\LogFile-.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                builder.AddSerilog(logger);
            })
            .AddSingleton<ILogger>(sp =>
            {
                return new LoggerConfiguration()
                    .CreateLogger();
            });
        }
    }
}