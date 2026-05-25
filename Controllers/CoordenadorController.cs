using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.DTOs;
using Hackathon_segunda_chamada.Models;
using Hackathon_segunda_chamada.Services;

namespace Hackathon_segunda_chamada.Controllers
{
    [Authorize(Roles = "Coordenador")]
    public class CoordenadorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IArquivoService _arquivoService;
        private readonly IRequerimentoService _requerimentoService;

        public CoordenadorController(
            AppDbContext context,
            IArquivoService arquivoService,
            IRequerimentoService requerimentoService)
        {
            _context = context;
            _arquivoService = arquivoService;
            _requerimentoService = requerimentoService;
        }

        public async Task<IActionResult> Painel(string? status)
        {
            var query = _context.RequerimentosSegundaChamada.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(r => r.Status == status);

            var requerimentos = await query
                .OrderByDescending(r => r.DataCriacao)
                .ToListAsync();

            return View(requerimentos);
        }

        [HttpPost]
        public async Task<IActionResult> Aprovar(int id)
        {
            var req = await _context.RequerimentosSegundaChamada.FindAsync(id);
            if (req != null)
            {
                req.Status = "Aprovado";
                req.MotivoRecusa = null;
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Requerimento aprovado com sucesso!";
            }
            return RedirectToAction("Painel");
        }

        [HttpPost]
        public async Task<IActionResult> Negar(int id, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                TempData["MensagemErro"] = "É obrigatório fornecer uma justificativa para recusar o requerimento.";
                return RedirectToAction("Painel");
            }

            var req = await _context.RequerimentosSegundaChamada.FindAsync(id);
            if (req != null)
            {
                req.Status = "Negado";
                req.MotivoRecusa = motivo;
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Requerimento recusado com sucesso.";
            }

            return RedirectToAction("Painel");
        }

        [HttpPost]
        public async Task<IActionResult> CriarParaAluno(
            int matriculaAluno,
            string nomeMateria,
            string motivo,
            string tipoAtestado,
            IFormFile arquivo)
        {
            if (matriculaAluno <= 0)
                return Json(new { sucesso = false, mensagem = "Informe uma matrícula válida." });

            if (string.IsNullOrWhiteSpace(nomeMateria))
                return Json(new { sucesso = false, mensagem = "Selecione uma matéria." });

            if (string.IsNullOrWhiteSpace(motivo))
                return Json(new { sucesso = false, mensagem = "O motivo é obrigatório." });

            if (string.IsNullOrWhiteSpace(tipoAtestado))
                return Json(new { sucesso = false, mensagem = "Selecione o tipo de atestado." });

            if (arquivo == null || arquivo.Length == 0)
                return Json(new { sucesso = false, mensagem = "Anexe o arquivo do atestado." });

            var aluno = await _context.Usuarios.FirstOrDefaultAsync(u => u.Matricula == matriculaAluno);
            if (aluno == null)
                return Json(new { sucesso = false, mensagem = "Aluno não encontrado no sistema." });

            if (aluno.Perfil != PerfilUsuario.AlunoEng && aluno.Perfil != PerfilUsuario.AlunoSI)
                return Json(new { sucesso = false, mensagem = "A matrícula informada não pertence a um aluno." });

            try
            {
                var urlAtestado = await _arquivoService.SalvarArquivo(arquivo);

                var dto = new CriarRequerimentoDto(
                    MatriculaAluno: matriculaAluno,
                    NomeMateria: nomeMateria,
                    Motivo: motivo,
                    TipoAtestado: tipoAtestado,
                    URLAtestado: urlAtestado
                );

                await _requerimentoService.CriarRequerimento(dto);

                return Json(new { sucesso = true, mensagem = "Requerimento cadastrado com sucesso!" });
            }
            catch (ArgumentException ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { sucesso = false, mensagem = "Erro interno ao salvar. Tente novamente." });
            }
        }
    }
}
