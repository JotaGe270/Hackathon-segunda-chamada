using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.DTOs;
using Hackathon_segunda_chamada.Models;
using Hackathon_segunda_chamada.Services;

namespace Hackathon_segunda_chamada.Controllers
{
    [Authorize]
    public class RequerimentoSegundaChamadaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRequerimentoService _requerimentoService;
        private readonly IArquivoService _arquivoService;

        public RequerimentoSegundaChamadaController(
            AppDbContext context,
            IRequerimentoService requerimentoService,
            IArquivoService arquivoService)
        {
            _context = context;
            _requerimentoService = requerimentoService;
            _arquivoService = arquivoService;
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return RedirectToAction("Dashboard", "Aluno");
        }

        // Recebe o formulário do aluno via fetch (multipart/form-data)
        [HttpPost]
        [Route("requerimento/criar")]
        [Authorize(Roles = "AlunoEng,AlunoSI")]
        public async Task<IActionResult> Criar(
            string nomeMateria,
            string motivo,
            string tipoAtestado,
            IFormFile arquivo)
        {
            var matriculaStr = User.FindFirstValue(ClaimTypes.Name);
            if (!int.TryParse(matriculaStr, out var matricula))
                return Json(new { sucesso = false, mensagem = "Sessão inválida. Faça login novamente." });

            if (string.IsNullOrWhiteSpace(nomeMateria))
                return Json(new { sucesso = false, mensagem = "Selecione uma matéria." });

            if (string.IsNullOrWhiteSpace(motivo))
                return Json(new { sucesso = false, mensagem = "O motivo é obrigatório." });

            if (string.IsNullOrWhiteSpace(tipoAtestado))
                return Json(new { sucesso = false, mensagem = "Selecione o tipo de atestado." });

            if (arquivo == null || arquivo.Length == 0)
                return Json(new { sucesso = false, mensagem = "Anexe o atestado comprobatório." });

            try
            {
                var urlAtestado = await _arquivoService.SalvarArquivo(arquivo);

                var dto = new CriarRequerimentoDto(
                    MatriculaAluno: matricula,
                    NomeMateria: nomeMateria,
                    Motivo: motivo,
                    TipoAtestado: tipoAtestado,
                    URLAtestado: urlAtestado
                );

                await _requerimentoService.CriarRequerimento(dto);

                return Json(new { sucesso = true, mensagem = "Requerimento enviado com sucesso!" });
            }
            catch (ArgumentException ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { sucesso = false, mensagem = "Erro interno ao salvar o requerimento. Tente novamente." });
            }
        }

        // Retorna as matérias do curso do aluno em formato JSON
        [HttpGet]
        public IActionResult BuscarMateriasPorMatricula(int matricula)
        {
            var aluno = _context.Usuarios.FirstOrDefault(u => u.Matricula == matricula);

            if (aluno == null)
                return Json(new { sucesso = false, mensagem = "Aluno não encontrado no sistema." });

            List<string> materias = new List<string>();

            if (aluno.Perfil == PerfilUsuario.AlunoEng)
                materias.AddRange(new[] { "Cálculo Diferencial e Integral I", "Física para Engenharia", "Algoritmos e Lógica de Programação", "Geometria Analítica" });
            else if (aluno.Perfil == PerfilUsuario.AlunoSI)
                materias.AddRange(new[] { "Fundamentos de Banco de Dados", "Redes de Computadores", "Desenvolvimento Web", "Engenharia de Requisitos" });
            else
                return Json(new { sucesso = false, mensagem = "A matrícula informada não pertence a um aluno." });

            return Json(new { sucesso = true, dados = materias });
        }
    }
}
