using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Hackathon_segunda_chamada.Services
{
    public class ArquivoService : IArquivoService
    {
        private readonly IWebHostEnvironment _env;

        private static readonly HashSet<string> _extensoesPermitidas =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".jpg", ".jpeg", ".png" };

        private static readonly HashSet<string> _mimeTypesPermitidos =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "application/pdf",
                "image/jpeg",
                "image/png"
            };

        private const long TamanhoMaximoBytes = 10 * 1024 * 1024; // 10 MB

        public ArquivoService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SalvarArquivo(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                throw new ArgumentException("Arquivo inválido ou vazio.");

            if (arquivo.Length > TamanhoMaximoBytes)
                throw new ArgumentException("O arquivo excede o tamanho máximo permitido de 10 MB.");

            var extensao = Path.GetExtension(arquivo.FileName);
            if (!_extensoesPermitidas.Contains(extensao))
                throw new ArgumentException("Tipo de arquivo não permitido.");

            if (!_mimeTypesPermitidos.Contains(arquivo.ContentType))
                throw new ArgumentException("Tipo de arquivo não permitido.");

            // Gerar nome seguro com GUID — nunca usar nome original (Requisito 4.2, 8.4)
            var nomeArquivo = $"{Guid.NewGuid()}{extensao.ToLowerInvariant()}";
            var pastaUploads = Path.Combine(_env.WebRootPath, "uploads");

            // Garantir que a pasta existe
            Directory.CreateDirectory(pastaUploads);

            var caminhoCompleto = Path.Combine(pastaUploads, nomeArquivo);

            await using var stream = new FileStream(caminhoCompleto, FileMode.Create);
            await arquivo.CopyToAsync(stream);

            return $"/uploads/{nomeArquivo}";
        }
    }
}
