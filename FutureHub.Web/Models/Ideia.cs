namespace FutureHub.Web.Models;

public class Ideia
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string AutorId { get; set; } = string.Empty;
    public string? MissaoId { get; set; }
    public double MediaNotas { get; set; } = 0.0;
    public int TotalAvaliacoes { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Usuario Autor { get; set; } = null!;
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
}
