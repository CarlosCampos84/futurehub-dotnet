namespace FutureHub.Web.Models;

public class Area
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;

    // Navigation Properties
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
