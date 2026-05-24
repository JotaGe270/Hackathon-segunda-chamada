namespace Hackathon_segunda_chamada.DTOs
{
    public record DadosAlunoDto(
        int Matricula,
        string NomeCompleto,
        string Curso,
        string Periodo,
        string Turno
    );
}
