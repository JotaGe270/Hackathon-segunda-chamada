using Microsoft.AspNetCore.Http;

namespace Hackathon_segunda_chamada.Services
{
    public interface IArquivoService
    {
        Task<string> SalvarArquivo(IFormFile arquivo);
    }
}
