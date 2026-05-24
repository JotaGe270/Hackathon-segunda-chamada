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
        public string Motivo { get; set; } // motivo da ausência informado pelo aluno

        [Required]
        public string TipoAtestado { get; set; } // medico, trabalho e obito

        [Required]
        public string URLAtestado { get; set; }
        [Required]
        public string Status { get; set; } = "Pendente"; // (sempre vai ser pendente quando for criado, até o Coordenador aprovar ou negar)

        public string? MotivoRecusa { get; set; } // só vai ser preenchido se o coordenador negar o requerimento, para dar um feedback pro aluno
        public DateTime DataCriacao { get; set; } = DateTime.Now;

    }
}
