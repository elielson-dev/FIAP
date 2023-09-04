using FiapDesafio;
using FiapDesafio.Config;
using FiapDesafio.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

ServiceCollection services = new();
services.AddConsoleConfiguration();

ServiceProvider serviceProvider = services.BuildServiceProvider();

var consultarService = serviceProvider.GetService<IConsultarService>();
var logger = serviceProvider.GetService<ILogger<ConsultarService>>();

var consultar = new Consultar(consultarService, logger);
await consultar.ExecutarAsync();
Console.ReadLine();