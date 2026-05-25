namespace Hackathon_segunda_chamada.DTOs
{
    public record AlunoComRequerimentoDto(
        int Matricula,
        string NomeMateria,
        string TipoAtestado,
        DateTime DataAprovacao
    );

    public record PainelProfessorDto(
        int MatriculaProfessor,
        List<AlunoComRequerimentoDto> AlunosEng,
        List<AlunoComRequerimentoDto> AlunosSI
    );
}
