using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.DTOs;
using Hackathon_segunda_chamada.Models;
using Microsoft.EntityFrameworkCore;

namespace Hackathon_segunda_chamada.Services
{
    public class RequerimentoService : IRequerimentoService
    {
        private readonly AppDbContext _context;

        private static readonly HashSet<string> _tiposValidos =
            new(StringComparer.OrdinalIgnoreCase) { "medico", "trabalho", "obito" };

        public RequerimentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RequerimentoDetalheDto> CriarRequerimento(CriarRequerimentoDto dto)
        {
            // Validar tipo de atestado (Requisito 3.6, Propriedade 5)
            if (!_tiposValidos.Contains(dto.TipoAtestado))
                throw new ArgumentException($"Tipo de atestado inválido: '{dto.TipoAtestado}'. Valores aceitos: medico, trabalho, obito.");

            var requerimento = new RequerimentoSegundaChamada
            {
                MatriculaAluno = dto.MatriculaAluno,
                NomeMateria = dto.NomeMateria,
                Motivo = dto.Motivo,
                TipoAtestado = dto.TipoAtestado,
                URLAtestado = dto.URLAtestado,
                Status = "Pendente",
                DataCriacao = DateTime.UtcNow
            };

            _context.RequerimentosSegundaChamada.Add(requerimento);
            await _context.SaveChangesAsync();

            return MapearParaDetalhe(requerimento);
        }

        public async Task<RequerimentoDetalheDto?> ObterPorId(int id)
        {
            // Rejeitar id inválido sem consultar banco (Requisito 6.3)
            if (id <= 0)
                return null;

            var requerimento = await _context.RequerimentosSegundaChamada
                .FirstOrDefaultAsync(r => r.Id == id);

            if (requerimento == null)
                return null;

            return MapearParaDetalhe(requerimento);
        }

        private static RequerimentoDetalheDto MapearParaDetalhe(RequerimentoSegundaChamada r) =>
            new(r.Id, r.MatriculaAluno, r.NomeMateria, r.Motivo,
                r.TipoAtestado, r.URLAtestado, r.Status, r.DataCriacao);
    }
}
