using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hackathon_segunda_chamada.Models
{
    public class RequerimentoSegundaChamada
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MatriculaAluno { get; set; }

        [Required]
        public String NomeMateria { get; set; }

        [Required]
        public string TipoAtestado { get; set; } // medico, trabalho e obto 

        [Required]
        public string URLAtestado { get; set; }
        [Required]
        public string Status { get; set; } = "Pendente"; // (sempre vai ser pendente quando for criado, até o Coordenador aprovar ou negar)

        public DateTime DataCriacao { get; set; } = DateTime.Now;

    }
}
