using Hackathon_segunda_chamada.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hackathon_segunda_chamada.Controllers
{
    [Authorize(Roles = "AlunoEng,AlunoSI")]
    [Route("Aluno")]
    public class AlunoController : Controller
    {
        private readonly IAlunoService _alunoService;

        public AlunoController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        // GET /Aluno/Dashboard
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var matricula = ObterMatricula();
            if (matricula == null)
                return RedirectToAction("Login", "Account");

            var dados = await _alunoService.ObterDadosAluno(matricula.Value);
            return View(dados);
        }

        // GET /aluno/materias
        [HttpGet("materias")]
        public async Task<IActionResult> ObterMaterias()
        {
            var matricula = ObterMatricula();
            if (matricula == null)
                return Unauthorized();

            var materias = await _alunoService.ObterMaterias(matricula.Value);
            return Json(materias);
        }

        // GET /aluno/requerimentos
        [HttpGet("requerimentos")]
        public async Task<IActionResult> ObterRequerimentos()
        {
            var matricula = ObterMatricula();
            if (matricula == null)
                return Unauthorized();

            var requerimentos = await _alunoService.ObterRequerimentos(matricula.Value);
            return Json(requerimentos);
        }

        private int? ObterMatricula()
        {
            var valor = User.FindFirstValue(ClaimTypes.Name);
            if (int.TryParse(valor, out var matricula))
                return matricula;
            return null;
        }
    }
}
