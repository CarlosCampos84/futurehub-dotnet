namespace FutureHub.Web.Models;

public class Usuario
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Role { get; set; } = "ROLE_USER"; // ROLE_USER ou ROLE_ADMIN
    public string? AreaInteresseId { get; set; }
    public int Pontos { get; set; } = 0;

    // Navigation Properties
    public Area? AreaInteresse { get; set; }
    public ICollection<Ideia> Ideias { get; set; } = new List<Ideia>();
    public ICollection<Ranking> Rankings { get; set; } = new List<Ranking>();
}
