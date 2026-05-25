namespace Hackathon_segunda_chamada.DTOs
{
    public record PerfilCoordenadorDto(
        int Matricula,
        string NomeCompleto,
        string Departamento,
        int TotalRequerimentosPendentes,
        int TotalRequerimentosAprovados,
        int TotalRequerimentosNegados,
        List<RequerimentoResumoDto> RequerimentosRecentes
    );
}
