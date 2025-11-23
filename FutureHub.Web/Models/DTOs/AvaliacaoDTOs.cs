using System.ComponentModel.DataAnnotations;

namespace FutureHub.Web.Models.DTOs;

public class AvaliacaoCreateDTO
{
    [Required(ErrorMessage = "O ID da ideia é obrigatório")]
    [StringLength(36)]
    public string IdeiaId { get; set; } = string.Empty;

    [Required(ErrorMessage = "A nota é obrigatória")]
    [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5")]
    public int Nota { get; set; }
}

public class AvaliacaoDTO
{
    public string Id { get; set; } = string.Empty;
    public string IdeiaId { get; set; } = string.Empty;
    public int Nota { get; set; }
    public DateTime DataAvaliacao { get; set; }
    public DateTime CreatedAt { get; set; }

    public static AvaliacaoDTO FromEntity(Avaliacao avaliacao)
    {
        return new AvaliacaoDTO
        {
            Id = avaliacao.Id,
            IdeiaId = avaliacao.IdeiaId,
            Nota = avaliacao.Nota,
            DataAvaliacao = avaliacao.DataAvaliacao,
            CreatedAt = avaliacao.CreatedAt
        };
    }
}
