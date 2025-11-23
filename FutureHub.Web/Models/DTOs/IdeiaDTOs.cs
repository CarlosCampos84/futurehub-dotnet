using System.ComponentModel.DataAnnotations;

namespace FutureHub.Web.Models.DTOs;

public class IdeiaCreateDTO
{
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(160, ErrorMessage = "O título deve ter no máximo 160 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres")]
    public string Descricao { get; set; } = string.Empty;
}

public class IdeiaUpdateDTO
{
    [StringLength(160, ErrorMessage = "O título deve ter no máximo 160 caracteres")]
    public string? Titulo { get; set; }

    [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres")]
    public string? Descricao { get; set; }
}

public class IdeiaDTO
{
    public string Id { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string AutorId { get; set; } = string.Empty;
    public string AutorNome { get; set; } = string.Empty;
    public double MediaNotas { get; set; }
    public int TotalAvaliacoes { get; set; }
    public DateTime CreatedAt { get; set; }

    public static IdeiaDTO FromEntity(Ideia ideia)
    {
        return new IdeiaDTO
        {
            Id = ideia.Id,
            Titulo = ideia.Titulo,
            Descricao = ideia.Descricao,
            AutorId = ideia.AutorId,
            AutorNome = ideia.Autor?.Nome ?? string.Empty,
            MediaNotas = ideia.MediaNotas,
            TotalAvaliacoes = ideia.TotalAvaliacoes,
            CreatedAt = ideia.CreatedAt
        };
    }
}
