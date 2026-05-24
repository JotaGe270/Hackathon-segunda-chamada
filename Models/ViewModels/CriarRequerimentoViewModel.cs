using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Hackathon_segunda_chamada.Models.ViewModels
{
    public class CriarRequerimentoViewModel
    {
        [Required(ErrorMessage = "A matéria é obrigatória.")]
        public string NomeMateria { get; set; } = string.Empty;

        [Required(ErrorMessage = "O motivo é obrigatório.")]
        [StringLength(500, ErrorMessage = "O motivo não pode ultrapassar 500 caracteres.")]
        public string Motivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de atestado é obrigatório.")]
        public string TipoAtestado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O arquivo é obrigatório.")]
        public IFormFile Arquivo { get; set; } = null!;
    }
}
