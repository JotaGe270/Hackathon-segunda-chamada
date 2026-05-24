using Hackathon_segunda_chamada.DTOs;

namespace Hackathon_segunda_chamada.Services
{
    public interface IRequerimentoService
    {
        /// <summary>
        /// Cria novo requerimento com status "Pendente".
        /// Precondição: dto.TipoAtestado ∈ {"medico","trabalho","obito"}
        /// </summary>
        Task<RequerimentoDetalheDto> CriarRequerimento(CriarRequerimentoDto dto);

        /// <summary>
        /// Retorna detalhe de um requerimento.
        /// Precondição: id > 0
        /// Pós-condição: retorna null se não encontrado
        /// </summary>
        Task<RequerimentoDetalheDto?> ObterPorId(int id);
    }
}
