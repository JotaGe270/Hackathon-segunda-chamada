using Hackathon_segunda_chamada.Models;

namespace Hackathon_segunda_chamada.DTOs
{
    public record UsuarioAutenticadoDto(
        int Id,
        int Matricula,
        PerfilUsuario Perfil
    );
}
