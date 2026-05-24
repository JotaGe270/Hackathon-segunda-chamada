using Hackathon_segunda_chamada.Data;
using Hackathon_segunda_chamada.DTOs;
using Hackathon_segunda_chamada.Models;
using Microsoft.EntityFrameworkCore;

namespace Hackathon_segunda_chamada.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DadosAlunoDto> ObterDadosAluno(int matricula)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Matricula == matricula)
                ?? throw new InvalidOperationException($"Usuário com matrícula {matricula} não encontrado.");

            var curso = usuario.Perfil switch
            {
                PerfilUsuario.AlunoEng => "Engenharia",
                PerfilUsuario.AlunoSI  => "Sistemas de Informação",
                _                      => "Curso não identificado"
            };

            return new DadosAlunoDto(
                Matricula: usuario.Matricula,
                NomeCompleto: $"Aluno {usuario.Matricula}",  // placeholder até modelo ter nome
                Curso: curso,
                Periodo: "N/A",   // placeholder
                Turno: "N/A"      // placeholder
            );
        }

        public async Task<List<MateriaDto>> ObterMaterias(int matricula)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Matricula == matricula);

            if (usuario == null)
                return new List<MateriaDto>();

            // Lista estática por curso (hardcoded até tabela de matérias existir)
            return usuario.Perfil switch
            {
                PerfilUsuario.AlunoEng => new List<MateriaDto>
                {
                    new("ENG101", "Cálculo I"),
                    new("ENG102", "Física I"),
                    new("ENG103", "Álgebra Linear"),
                    new("ENG104", "Programação para Engenharia"),
                    new("ENG105", "Química Geral"),
                },
                PerfilUsuario.AlunoSI => new List<MateriaDto>
                {
                    new("SI101", "Algoritmos e Estruturas de Dados"),
                    new("SI102", "Banco de Dados"),
                    new("SI103", "Programação Orientada a Objetos"),
                    new("SI104", "Engenharia de Software"),
                    new("SI105", "Redes de Computadores"),
                },
                _ => new List<MateriaDto>()
            };
        }

        public async Task<List<RequerimentoResumoDto>> ObterRequerimentos(int matricula)
        {
            // Ordenado por DataCriacao DESC (Propriedade 10)
            return await _context.RequerimentosSegundaChamada
                .Where(r => r.MatriculaAluno == matricula)
                .OrderByDescending(r => r.DataCriacao)
                .Select(r => new RequerimentoResumoDto(
                    r.Id,
                    r.NomeMateria,
                    r.TipoAtestado,
                    r.Status,
                    r.DataCriacao))
                .ToListAsync();
        }
    }
}
