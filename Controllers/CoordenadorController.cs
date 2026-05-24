using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.Models;

namespace Hackathon_segunda_chamada.Controllers
{
    // apenas usuários com Perfil "Coordenador" 
    [Authorize(Roles = "Coordenador")]
    public class CoordenadorController : Controller
    {
        private readonly AppDbContext _context;

        public CoordenadorController(AppDbContext context)
        {
            _context = context;
        }

        // painel com todos os pedidos feitos na faculdade
        public async Task<IActionResult> Painel()
        {
            var requerimentos = await _context.RequerimentosSegundaChamada
                .OrderByDescending(r => r.DataCriacao)
                .ToListAsync();

            return View(requerimentos);
        }

        // Action para Aprovar o pedido
        [HttpPost]
        public async Task<IActionResult> Aprovar(int id)
        {
            var req = await _context.RequerimentosSegundaChamada.FindAsync(id);
            if (req != null)
            {
                req.Status = "Aprovado";
                req.MotivoRecusa = null; // Limpa o campo caso tivesse alguma coisa antes
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Requerimento aprovado com sucesso!";
            }
            return RedirectToAction("Painel");
        }

        //  Action para Negar o pedido 
        [HttpPost]
        public async Task<IActionResult> Negar(int id, string motivo)
        {
            // o motivo é obrigatório para recusar um pedido, então verificamos se ele foi fornecido
            if (string.IsNullOrWhiteSpace(motivo))
            {
                TempData["MensagemErro"] = "É obrigatório fornecer uma justificativa para recusar o requerimento.";
                return RedirectToAction("Painel");
            }

            var req = await _context.RequerimentosSegundaChamada.FindAsync(id);
            if (req != null)
            {
                req.Status = "Negado";
                req.MotivoRecusa = motivo; // Grava o motivo obrigatório
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Requerimento recusado com sucesso.";
            }

            return RedirectToAction("Painel");
        }
    }
}