using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FutureHub.Web.Models;

namespace FutureHub.Web.Data.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Nome da tabela Oracle
        builder.ToTable("T_FH_USUARIOS");

        // Chave primária
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("ID")
            .HasMaxLength(36)
            .IsRequired();

        // Propriedades
        builder.Property(u => u.Nome)
            .HasColumnName("NOME")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("EMAIL")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.SenhaHash)
            .HasColumnName("SENHA_HASH")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("ROLE")
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("ROLE_USER");

        builder.Property(u => u.AreaInteresseId)
            .HasColumnName("AREA_INTERESSE_ID")
            .HasMaxLength(36);

        builder.Property(u => u.Pontos)
            .HasColumnName("PONTOS")
            .HasDefaultValue(0);

        // Índices
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("UK_USUARIO_EMAIL");

        builder.HasIndex(u => u.AreaInteresseId)
            .HasDatabaseName("IDX_USUARIO_AREA");

        // Relacionamentos
        builder.HasOne(u => u.AreaInteresse)
            .WithMany(a => a.Usuarios)
            .HasForeignKey(u => u.AreaInteresseId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_USUARIO_AREA");

        builder.HasMany(u => u.Ideias)
            .WithOne(i => i.Autor)
            .HasForeignKey(i => i.AutorId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_IDEIA_AUTOR");

        builder.HasMany(u => u.Rankings)
            .WithOne(r => r.Usuario)
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_RANKING_USUARIO");
    }
}
