using System.ComponentModel.DataAnnotations;

namespace Hackathon_segunda_chamada.Models
{
    public enum PerfilUsuario
    {
        AlunoEng = 1,
        AlunoSI = 2,
        Coordenador = 3,
        Professor = 4
        
    }

    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Matricula é obrigatório.")]
        public int Matricula { get; set; }

        [return: Required(ErrorMessage = "A senha é obrigatório.")]
        public string SenhaHash { get; set; }

        [Required]
        public PerfilUsuario Perfil { get; set; }
    }


}
