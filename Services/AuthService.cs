using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Hackathon_segunda_chamada.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioAutenticadoDto?> ValidarCredenciais(int matricula, string senha)
        {
            // Rejeitar matrícula inválida sem consultar o banco (Requisito 1.6)
            if (matricula <= 0)
                return null;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Matricula == matricula);

            if (usuario == null)
                return null;

            // Comparação de senha em texto claro (dev only — usar hash em produção)
            if (usuario.SenhaHash != senha)
                return null;

            return new UsuarioAutenticadoDto(usuario.Id, usuario.Matricula, usuario.Perfil);
        }
    }
}
