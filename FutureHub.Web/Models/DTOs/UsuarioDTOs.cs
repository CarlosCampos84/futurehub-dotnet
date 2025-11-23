using System.ComponentModel.DataAnnotations;

namespace FutureHub.Web.Models.DTOs;

public class UsuarioCreateDTO
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(200, ErrorMessage = "O email deve ter no máximo 200 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [StringLength(36)]
    public string? AreaInteresseId { get; set; }
}

public class UsuarioUpdateDTO
{
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string? Nome { get; set; }

    [StringLength(36)]
    public string? AreaInteresseId { get; set; }
}

public class UsuarioDTO
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? AreaInteresseId { get; set; }
    public string? AreaInteresseNome { get; set; }
    public int Pontos { get; set; }

    public static UsuarioDTO FromEntity(Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Role = usuario.Role,
            AreaInteresseId = usuario.AreaInteresseId,
            AreaInteresseNome = usuario.AreaInteresse?.Nome,
            Pontos = usuario.Pontos
        };
    }
}
