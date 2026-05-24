using System.ComponentModel.DataAnnotations;

namespace Hackathon_segunda_chamada.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        public int Matricula { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;
    }
}
