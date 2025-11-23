using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FutureHub.Web.Models;

namespace FutureHub.Web.Data.Mappings;

public class AreaMapping : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        // Nome da tabela Oracle
        builder.ToTable("T_FH_AREAS");

        // Chave primária
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("ID")
            .HasMaxLength(36)
            .IsRequired();

        // Propriedades
        builder.Property(a => a.Nome)
            .HasColumnName("NOME")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Descricao)
            .HasColumnName("DESCRICAO")
            .HasMaxLength(500)
            .IsRequired();

        // Índices
        builder.HasIndex(a => a.Nome)
            .IsUnique()
            .HasDatabaseName("UK_AREA_NOME");
    }
}
