using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FutureHub.Web.Models;

namespace FutureHub.Web.Data.Mappings;

public class IdeiaMapping : IEntityTypeConfiguration<Ideia>
{
    public void Configure(EntityTypeBuilder<Ideia> builder)
    {
        // Nome da tabela Oracle
        builder.ToTable("T_FH_IDEIAS");

        // Chave primária
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnName("ID")
            .HasMaxLength(36)
            .IsRequired();

        // Propriedades
        builder.Property(i => i.Titulo)
            .HasColumnName("TITULO")
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(i => i.Descricao)
            .HasColumnName("DESCRICAO")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(i => i.AutorId)
            .HasColumnName("AUTOR_ID")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(i => i.MediaNotas)
            .HasColumnName("MEDIA_NOTAS")
            .HasPrecision(3, 2)
            .HasDefaultValue(0.0);

        builder.Property(i => i.TotalAvaliacoes)
            .HasColumnName("TOTAL_AVALIACOES")
            .HasDefaultValue(0);

        builder.Property(i => i.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired();

        // Índices
        builder.HasIndex(i => i.AutorId)
            .HasDatabaseName("IDX_IDEIA_AUTOR");

        builder.HasIndex(i => i.CreatedAt)
            .HasDatabaseName("IDX_IDEIA_CREATED");

        builder.HasIndex(i => i.MediaNotas)
            .HasDatabaseName("IDX_IDEIA_MEDIA");

        // Relacionamento com Autor já configurado em UsuarioMapping
        
        // Relacionamento com Avaliacoes
        builder.HasMany(i => i.Avaliacoes)
            .WithOne(a => a.Ideia)
            .HasForeignKey(a => a.IdeiaId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_AVALIACAO_IDEIA");
    }
}
