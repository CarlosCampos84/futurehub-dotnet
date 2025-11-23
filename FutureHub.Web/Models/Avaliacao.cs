namespace FutureHub.Web.Models;

public class Avaliacao
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string IdeiaId { get; set; } = string.Empty;
    public int Nota { get; set; } // 1 a 5
    public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Ideia Ideia { get; set; } = null!;
}
