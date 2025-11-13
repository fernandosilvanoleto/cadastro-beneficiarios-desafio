using Desafio_Tecnico.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico.Infraestructure.Data
{
    public class AppDbContext : DbContext
    {   
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<PlanoModel> Planos { get; set; }
    public DbSet<BeneficiarioModel> Beneficiarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            // serve para modelagem de banco de dados

            builder
                .Entity<PlanoModel>(p => {
                    p.HasKey(pm => pm.Id);

                    p.HasIndex(pm => pm.Nome)
                        .IsUnique();

                    p.HasIndex(pm => pm.Codigo_registro_ans)
                        .IsUnique();
                });

            builder
                .Entity<BeneficiarioModel>(b => {
                    b.HasKey(bm => bm.Id);

                    b.HasOne(bm => bm.Plano)
                        .WithMany(plano => plano.Beneficiarios)
                        .HasForeignKey(bm => bm.PlanoId)
                        .OnDelete(DeleteBehavior.Restrict); // não permite uma exclusão em massa de beneficiário, caso um plano seja excluído

                   b.Property(bm => bm.DataNascimento)
                        .HasColumnType("date")
                        .IsRequired();

                    b.HasIndex(bm => bm.Cpf)
                        .IsUnique();
                });

            base.OnModelCreating(builder);
        }

    }
}
