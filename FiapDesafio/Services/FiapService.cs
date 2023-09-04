using FiapDesafio.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FiapDesafio.Services
{
    public class FiapService : IFiapService
    {
        private readonly HttpClient _client;
        private readonly string _endPoint;
        private static readonly MediaTypeHeaderValue contentType = new("application/json");
        private readonly ILogger<FiapService> _logger;

        public FiapService(HttpClient client, IOptions<FiapApiSettings> workFlowSettings, ILogger<FiapService> logger)
        {
            _client = client;
            _endPoint = workFlowSettings.Value.EndPoint;
            _logger = logger;
        }

        public async Task<bool> ConsultarKey(RequestFiap request)
        {            
            try
            {
                var json = ObterJson(request);
                var content = new StringContent(json);
                content.Headers.ContentType = contentType;
                var httpResult = await _client.PostAsync($"{_endPoint}", content);

                var httpContentResult = await httpResult.Content.ReadAsStringAsync();

                if (httpResult.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                
                return false;
            }
            catch (Exception ex)
            {
                
                _logger.LogError("Ocorreu um erro ao consumir a api. Key: {key} - {mensagem}", request.Key, ex.Message);
                return false;
            }            
        }

        private string ObterJson<T>(T data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            return JsonSerializer.Serialize(data, options);
        }
    }
}
