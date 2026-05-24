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

        // abre a tela de Login 
        [HttpGet]
        public IActionResult Login()
        {
            // Se o cara já estiver logado e tentar acessar a tela de login, manda pra home
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // recebe os dados do formulário quando o usuário clica em "Entrar" (POST)
        [HttpPost]
        public async Task<IActionResult> Login(int matricula, string senha)
        {
            // busca o usuário no banco
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Matricula == matricula && u.SenhaHash == senha);

            if (usuario == null)
            {
                // mensagem de erro
                ViewBag.Erro = "Matrícula ou senha inválidos!";
                return View();
            }

            //  identidade do usuário com as informações dele
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Matricula.ToString()),
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString()) // aluno, coordenador, professor
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Efetua o login e gera o cookie no navegador
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redireciona para o dashboard correto por perfil
            return usuario.Perfil switch
            {
                PerfilUsuario.AlunoEng or PerfilUsuario.AlunoSI
                    => RedirectToAction("Dashboard", "Aluno"),
                PerfilUsuario.Coordenador
                    => RedirectToAction("Painel", "Coordenador"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        // Faz o Logout e destrói o cookie
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // Tela de "vc não tem permissão para acessar isso"
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}