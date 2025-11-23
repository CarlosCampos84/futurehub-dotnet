using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FutureHub.Web.Models;

namespace FutureHub.Web.Data.Mappings;

public class AvaliacaoMapping : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        // Nome da tabela Oracle
        builder.ToTable("T_FH_AVALIACOES");

        // Chave primária
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("ID")
            .HasMaxLength(36)
            .IsRequired();

        // Propriedades
        builder.Property(a => a.IdeiaId)
            .HasColumnName("IDEIA_ID")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(a => a.Nota)
            .HasColumnName("NOTA")
            .IsRequired();

        builder.Property(a => a.DataAvaliacao)
            .HasColumnName("DATA_AVALIACAO")
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired();

        // Índices
        builder.HasIndex(a => a.IdeiaId)
            .HasDatabaseName("IDX_AVALIACAO_IDEIA");

        builder.HasIndex(a => a.DataAvaliacao)
            .HasDatabaseName("IDX_AVALIACAO_DATA");

        // Relacionamento com Ideia já configurado em IdeiaMapping
    }
}
