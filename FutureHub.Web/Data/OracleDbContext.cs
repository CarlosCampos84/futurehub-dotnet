using Microsoft.EntityFrameworkCore;
using FutureHub.Web.Models;
using FutureHub.Web.Data.Mappings;

namespace FutureHub.Web.Data;

public class OracleDbContext : DbContext
{
    public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Ideia> Ideias { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<Ranking> Rankings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações de mapping
        modelBuilder.ApplyConfiguration(new UsuarioMapping());
        modelBuilder.ApplyConfiguration(new AreaMapping());
        modelBuilder.ApplyConfiguration(new IdeiaMapping());
        modelBuilder.ApplyConfiguration(new AvaliacaoMapping());
        modelBuilder.ApplyConfiguration(new RankingMapping());
    }
}
