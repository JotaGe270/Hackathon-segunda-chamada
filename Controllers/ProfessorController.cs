using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.DTOs;
using Hackathon_segunda_chamada.Models;

namespace Hackathon_segunda_chamada.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {
        private readonly AppDbContext _context;

        public ProfessorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Painel()
        {
            var matriculaLogada = User.FindFirstValue(ClaimTypes.Name);
            if (matriculaLogada == null || !int.TryParse(matriculaLogada, out var matricula))
                return RedirectToAction("Login", "Account");

            var matriculasEng = await _context.Usuarios
                .Where(u => u.Perfil == PerfilUsuario.AlunoEng)
                .Select(u => u.Matricula)
                .ToListAsync();

            var matriculasSI = await _context.Usuarios
                .Where(u => u.Perfil == PerfilUsuario.AlunoSI)
                .Select(u => u.Matricula)
                .ToListAsync();

            var alunosEng = await _context.RequerimentosSegundaChamada
                .Where(r => r.Status == "Aprovado" && matriculasEng.Contains(r.MatriculaAluno))
                .OrderByDescending(r => r.DataCriacao)
                .Select(r => new AlunoComRequerimentoDto(r.MatriculaAluno, r.NomeMateria, r.TipoAtestado, r.DataCriacao))
                .ToListAsync();

            var alunosSI = await _context.RequerimentosSegundaChamada
                .Where(r => r.Status == "Aprovado" && matriculasSI.Contains(r.MatriculaAluno))
                .OrderByDescending(r => r.DataCriacao)
                .Select(r => new AlunoComRequerimentoDto(r.MatriculaAluno, r.NomeMateria, r.TipoAtestado, r.DataCriacao))
                .ToListAsync();

            return View(new PainelProfessorDto(matricula, alunosEng, alunosSI));
        }
    }
}
