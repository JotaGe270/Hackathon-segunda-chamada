using Hackathon_segunda_chamada.DTOs;

namespace Hackathon_segunda_chamada.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Valida matrícula + senha. Retorna null se credenciais inválidas.
        /// Precondição: matricula > 0
        /// </summary>
        Task<UsuarioAutenticadoDto?> ValidarCredenciais(int matricula, string senha);
    }
}
