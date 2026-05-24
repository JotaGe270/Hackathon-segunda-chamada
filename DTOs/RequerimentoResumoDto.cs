namespace Hackathon_segunda_chamada.DTOs
{
    public record RequerimentoResumoDto(
        int Id,
        string NomeMateria,
        string TipoAtestado,
        string Status,
        DateTime DataCriacao
    );
}
