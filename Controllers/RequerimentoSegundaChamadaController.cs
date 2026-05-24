using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.Models;

namespace Hackathon_segunda_chamada.Controllers
{
    // Garante que só usuários logados consigam acessar essa área
    [Authorize]
    public class RequerimentoSegundaChamadaController : Controller
    {
        private readonly AppDbContext _context;

        public RequerimentoSegundaChamadaController(AppDbContext context)
        {
            _context = context;
        }

        // tela do formulário (GET)
        [HttpGet]
        public IActionResult Novo()
        {
            var perfilDoUsuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            List<string> materiasDoPeriodo = new List<string>();

            // Preenche as matérias direto se for aluno
            if (perfilDoUsuario == PerfilUsuario.AlunoEng.ToString())
            {
                materiasDoPeriodo.AddRange(new[] { "Cálculo Diferencial e Integral I", "Física para Engenharia", "Algoritmos e Lógica de Programação", "Geometria Analítica" });
                ViewBag.Materias = new SelectList(materiasDoPeriodo);
            }
            else if (perfilDoUsuario == PerfilUsuario.AlunoSI.ToString())
            {
                materiasDoPeriodo.AddRange(new[] { "Fundamentos de Banco de Dados", "Redes de Computadores", "Desenvolvimento Web", "Engenharia de Requisitos" });
                ViewBag.Materias = new SelectList(materiasDoPeriodo);
            }
            else if (perfilDoUsuario == PerfilUsuario.Coordenador.ToString())
            {
                ViewBag.Materias = new SelectList(new List<string>());
            }

            return View();
        }

        // envio do formulário (POST)
        [HttpPost]
        public async Task<IActionResult> Novo(RequerimentoSegundaChamada requerimento)
        {
            if (User.IsInRole(PerfilUsuario.Coordenador.ToString()))
            {
                // Coordenador precisa digitar uma matrícula válida
                if (requerimento.MatriculaAluno <= 0)
                {
                    TempData["MensagemErro"] = "O Coordenador precisa informar uma matrícula válida.";
                    return RedirectToAction("Novo");
                }
            }
            else
            {
                // Aluno tem a matrícula puxada do Cookie de segurança
                var matriculaLogada = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (matriculaLogada != null)
                {
                    requerimento.MatriculaAluno = int.Parse(matriculaLogada);
                }
            }

            _context.RequerimentosSegundaChamada.Add(requerimento);
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = "Requerimento cadastrado com sucesso!";

            if (User.IsInRole(PerfilUsuario.Coordenador.ToString()))
            {
                return RedirectToAction("Painel", "Coordenador");
            }
            return RedirectToAction("Index", "Home");
        }

        //  Retorna as matérias do curso do aluno em formato JSON
        [HttpGet]
        public IActionResult BuscarMateriasPorMatricula(int matricula)
        {
            var aluno = _context.Usuarios.FirstOrDefault(u => u.Matricula == matricula);

            if (aluno == null)
            {
                return Json(new { sucesso = false, mensagem = "Aluno não encontrado no sistema." });
            }

            List<string> materias = new List<string>();

            if (aluno.Perfil == PerfilUsuario.AlunoEng)
            {
                materias.AddRange(new[] { "Cálculo Diferencial e Integral I", "Física para Engenharia", "Algoritmos e Lógica de Programação", "Geometria Analítica" });
            }
            else if (aluno.Perfil == PerfilUsuario.AlunoSI)
            {
                materias.AddRange(new[] { "Fundamentos de Banco de Dados", "Redes de Computadores", "Desenvolvimento Web", "Engenharia de Requisitos" });
            }
            else
            {
                return Json(new { sucesso = false, mensagem = "A matrícula informada não pertence a um aluno." });
            }

            return Json(new { sucesso = true, dados = materias });
        }
    }
}