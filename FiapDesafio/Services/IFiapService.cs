namespace FiapDesafio.Services
{
    public interface IFiapService
    {
        Task<bool> ConsultarKey(RequestFiap request);
    }
}
