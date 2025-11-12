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

    }
}
