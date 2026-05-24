namespace Hackathon_segunda_chamada.DTOs
{
    public record CriarRequerimentoDto(
        int MatriculaAluno,
        string NomeMateria,
        string Motivo,
        string TipoAtestado,
        string URLAtestado
    );
}
