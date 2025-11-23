using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FutureHub.Web.Models;

namespace FutureHub.Web.Data.Mappings;

public class RankingMapping : IEntityTypeConfiguration<Ranking>
{
    public void Configure(EntityTypeBuilder<Ranking> builder)
    {
        // Nome da tabela Oracle
        builder.ToTable("T_FH_RANKINGS");

        // Chave primária
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("ID")
            .HasMaxLength(36)
            .IsRequired();

        // Propriedades
        builder.Property(r => r.UsuarioId)
            .HasColumnName("USUARIO_ID")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(r => r.PontuacaoTotal)
            .HasColumnName("PONTUACAO_TOTAL")
            .HasDefaultValue(0);

        builder.Property(r => r.Periodo)
            .HasColumnName("PERIODO")
            .HasMaxLength(7)
            .IsRequired();

        // Índices
        builder.HasIndex(r => r.UsuarioId)
            .HasDatabaseName("IDX_RANKING_USUARIO");

        builder.HasIndex(r => r.Periodo)
            .HasDatabaseName("IDX_RANKING_PERIODO");

        builder.HasIndex(r => r.PontuacaoTotal)
            .HasDatabaseName("IDX_RANKING_PONTUACAO");

        // Índice composto único para evitar duplicatas
        builder.HasIndex(r => new { r.UsuarioId, r.Periodo })
            .IsUnique()
            .HasDatabaseName("UK_RANKING_USUARIO_PERIODO");

        // Relacionamento com Usuario já configurado em UsuarioMapping
    }
}
