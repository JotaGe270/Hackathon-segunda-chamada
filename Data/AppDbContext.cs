using Microsoft.EntityFrameworkCore;
using Hackathon_segunda_chamada.Models;

namespace Hackathon_segunda_chamada.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RequerimentoSegundaChamada> RequerimentosSegundaChamada { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Matricula)
                .IsUnique();

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Matricula = 1, SenhaHash = "123", Perfil = PerfilUsuario.AlunoEng },
                new Usuario { Id = 2, Matricula = 2, SenhaHash = "123", Perfil = PerfilUsuario.AlunoSI },
                new Usuario { Id = 3, Matricula = 3, SenhaHash = "123", Perfil = PerfilUsuario.Coordenador },
                new Usuario { Id = 4, Matricula = 4, SenhaHash = "123", Perfil = PerfilUsuario.Professor }
            );
        }
    }
}
