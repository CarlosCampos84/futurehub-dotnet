namespace FutureHub.Web.Models;

public class Ranking
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UsuarioId { get; set; } = string.Empty;
    public int PontuacaoTotal { get; set; } = 0;
    public string Periodo { get; set; } = string.Empty; // Formato: YYYY-MM

    // Navigation Properties
    public Usuario Usuario { get; set; } = null!;
}
