using Hackathon_segunda_chamada.DTOs;

namespace Hackathon_segunda_chamada.Services
{
    public interface IAlunoService
    {
        /// <summary>
        /// Retorna dados de perfil do aluno derivados do enum Perfil.
        /// Precondição: matricula existe no banco
        /// </summary>
        Task<DadosAlunoDto> ObterDadosAluno(int matricula);

        /// <summary>
        /// Retorna lista de matérias do aluno (hardcoded por curso).
        /// Pós-condição: lista pode ser vazia, nunca null
        /// </summary>
        Task<List<MateriaDto>> ObterMaterias(int matricula);

        /// <summary>
        /// Retorna histórico de requerimentos do aluno.
        /// Pós-condição: lista ordenada por DataCriacao DESC (Propriedade 10)
        /// </summary>
        Task<List<RequerimentoResumoDto>> ObterRequerimentos(int matricula);
    }
}
