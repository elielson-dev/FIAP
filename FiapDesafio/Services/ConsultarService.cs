using Microsoft.Extensions.Logging;

namespace FiapDesafio.Services
{
    public class ConsultarService : IConsultarService
    {
        
        private readonly ILogger<ConsultarService> _logger;
        private readonly IFiapService _fiapService;
        private readonly string caracteresIniciais = "AôDEcHòÕIghiãRSQÀàöVWlmXbjkyoMnPÄêèpÙîÌÃYZaJÛKdxN OefËëÜíÎúÈFÚùtuüÇBCÍûÓszÁÂwéÊrâLÅTUGáåóÔqäÉÒõÖvçÑñ";
        private readonly string caracteressFinais = "AôDEcHòÕIghiãRSQÀàöVWlmXbjkyoMnPÄêèpÙîÌÃYZaJÛKdxN OefËëÜíÎúÈFÚùtuüÇBCÍûÓszÁÂwéÊrâLÅTUGáåóÔqäÉÒõÖvçÑñ";
        private readonly List<int> numeros = new();
        private bool _encontrado = false;        
        private string mensagem = string.Empty;

        public ConsultarService(         
            ILogger<ConsultarService> logger,            
            IFiapService fiapService)
        {
            _logger = logger;            
            _fiapService = fiapService;
            for (int j = 0; j <= 10000; j++)
            {
                numeros.Add(j);
            }
        }

        public async Task ConsultarAsync()
        {
            try
            {
                var executa1 = ExecutaTask(0, 33, 0);
                var executa2 = ExecutaTask(34, 68,0);
                var executa3 = ExecutaTask(69, 99,0);

                await Task.WhenAll(executa1, executa2, executa3);

                Console.WriteLine($"{mensagem}");
                _logger.LogInformation("{msg}", mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao realizar a consulta {msg}", ex.Message);
            }
        }

        private async Task<bool> ConsultarKey(RequestFiap request)
        {
            return await _fiapService.ConsultarKey(request);
        }

        private async Task ExecutaTask(int countCaracterInicial, int posicaoCaracterFinal, int countCaracterFinal)
        {
            while (!_encontrado && countCaracterInicial <= posicaoCaracterFinal)
            {
                var caracterInicial = caracteresIniciais.Substring(countCaracterInicial, 1);
                Console.WriteLine(caracteresIniciais.Substring(countCaracterInicial, 1));
                while (!_encontrado && countCaracterFinal <= caracteressFinais.Length - 1)
                {
                    var caracterFinal = caracteressFinais.Substring(countCaracterFinal, 1);

                    int count = 0;
                    while (!_encontrado && count <= 10000)
                    {
                        var num = numeros.Skip(count).Take(1000).ToList();
                        await Task.Run(() =>
                        {
                            Parallel.ForEach(num, async (numero, state) =>
                            {
                                var key = $"{caracterInicial}{numero}{caracterFinal}".Trim();
                                Console.WriteLine($"Testando {key}");
                                if (await ConsultarKey(new RequestFiap { Key = $"{key}" }))
                                {
                                    mensagem = $"Encontrou: {key}";
                                    _encontrado = true;
                                    state.Stop();
                                }
                            });
                            count += 1000;
                        });
                    }
                    countCaracterFinal++;
                }
                countCaracterInicial++;
                countCaracterFinal = 0;
            }
        }
    }
}
