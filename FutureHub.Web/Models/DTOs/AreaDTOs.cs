using System.ComponentModel.DataAnnotations;

namespace FutureHub.Web.Models.DTOs;

public class AreaCreateDTO
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
    public string Descricao { get; set; } = string.Empty;
}

public class AreaUpdateDTO
{
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string? Nome { get; set; }

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
    public string? Descricao { get; set; }
}

public class AreaDTO
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;

    public static AreaDTO FromEntity(Area area)
    {
        return new AreaDTO
        {
            Id = area.Id,
            Nome = area.Nome,
            Descricao = area.Descricao
        };
    }
}
