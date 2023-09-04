using FiapDesafio.Services;
using Microsoft.Extensions.Logging;

namespace FiapDesafio
{
    public class Consultar
    {
        private readonly IConsultarService _service;
        private readonly ILogger<ConsultarService> _logger;

        public Consultar(IConsultarService service, ILogger<ConsultarService> logger)
        {
            _service = service;
            _logger = logger;
        }
        public async Task ExecutarAsync()
        {
            try
            {
                _logger.Log(LogLevel.Information, "Iniciando execucao");
                await _service.ConsultarAsync();
                _logger.Log(LogLevel.Information, "Fim da execucao");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Fim da execucao. Excecao: {ex}", ex.Message);
                throw;
            }
        }
    }
}
