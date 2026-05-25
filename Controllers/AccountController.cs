using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.Models;

namespace Hackathon_segunda_chamada.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirecionarPorPerfil();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(int matricula, string senha)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Matricula == matricula && u.SenhaHash == senha);

            if (usuario == null)
            {
                ViewBag.Erro = "Matrícula ou senha inválidos!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Matricula.ToString()),
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirecionarPorPerfil(usuario.Perfil);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirecionarPorPerfil(PerfilUsuario? perfil = null)
        {
            if (perfil == null && Enum.TryParse<PerfilUsuario>(
                User.FindFirst(ClaimTypes.Role)?.Value, out var p))
            {
                perfil = p;
            }

            return perfil switch
            {
                PerfilUsuario.AlunoEng or PerfilUsuario.AlunoSI => RedirectToAction("Dashboard", "Aluno"),
                PerfilUsuario.Coordenador                        => RedirectToAction("Painel", "Coordenador"),
                PerfilUsuario.Professor                          => RedirectToAction("Painel", "Professor"),
                _                                                => View("Login")
            };
        }
    }
}
