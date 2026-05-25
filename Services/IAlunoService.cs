using Hackathon_segunda_chamada.DTOs;

namespace Hackathon_segunda_chamada.Services
{
    public interface IAlunoService
    {
        Task<DadosAlunoDto> ObterDadosAluno(int matricula);
        Task<List<MateriaDto>> ObterMaterias(int matricula);
        Task<List<RequerimentoResumoDto>> ObterRequerimentos(int matricula);
    }
}
