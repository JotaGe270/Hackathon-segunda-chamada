namespace Hackathon_segunda_chamada.DTOs
{
    public record RequerimentoDetalheDto(
        int Id,
        int MatriculaAluno,
        string NomeMateria,
        string Motivo,
        string TipoAtestado,
        string URLAtestado,
        string Status,
        DateTime DataCriacao
    );
}
