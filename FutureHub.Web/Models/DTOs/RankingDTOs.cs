namespace FutureHub.Web.Models.DTOs;

public class RankingDTO
{
    public string Id { get; set; } = string.Empty;
    public string UsuarioId { get; set; } = string.Empty;
    public string UsuarioNome { get; set; } = string.Empty;
    public int PontuacaoTotal { get; set; }
    public string Periodo { get; set; } = string.Empty;

    public static RankingDTO FromEntity(Ranking ranking)
    {
        return new RankingDTO
        {
            Id = ranking.Id,
            UsuarioId = ranking.UsuarioId,
            UsuarioNome = ranking.Usuario?.Nome ?? string.Empty,
            PontuacaoTotal = ranking.PontuacaoTotal,
            Periodo = ranking.Periodo
        };
    }
}

public class RankingDetailDTO
{
    public string UsuarioId { get; set; } = string.Empty;
    public string UsuarioNome { get; set; } = string.Empty;
    public int PontuacaoTotal { get; set; }
    public int IdeiasPublicadas { get; set; }
    public double MediaAvaliacoes { get; set; }
}
