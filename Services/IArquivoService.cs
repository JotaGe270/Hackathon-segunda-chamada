using Microsoft.AspNetCore.Http;

namespace Hackathon_segunda_chamada.Services
{
    public interface IArquivoService
    {
        /// <summary>
        /// Valida e persiste arquivo no servidor.
        /// Precondição: arquivo não nulo, tamanho ≤ 10 MB,
        ///              extensão ∈ {".pdf", ".jpg", ".jpeg", ".png"}
        /// Pós-condição: arquivo salvo em wwwroot/uploads/{guid}{extensao}
        ///               retorna URL relativa acessível publicamente
        /// </summary>
        Task<string> SalvarArquivo(IFormFile arquivo);
    }
}
