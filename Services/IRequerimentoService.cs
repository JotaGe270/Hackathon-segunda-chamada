using Hackathon_segunda_chamada.DTOs;

namespace Hackathon_segunda_chamada.Services
{
    public interface IRequerimentoService
    {
        Task<RequerimentoDetalheDto> CriarRequerimento(CriarRequerimentoDto dto);
        Task<RequerimentoDetalheDto?> ObterPorId(int id);
    }
}
