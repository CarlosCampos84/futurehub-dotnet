using FutureHub.Web.Models;

namespace FutureHub.Web.Repositories.Interfaces;

public interface IRankingRepository
{
    Task<Ranking?> GetByIdAsync(string id);
    Task<Ranking?> GetByUsuarioAndPeriodoAsync(string usuarioId, string periodo);
    Task<IEnumerable<Ranking>> GetByPeriodoAsync(string periodo);
    Task<IEnumerable<Ranking>> GetTopRankingsAsync(int limit = 10);
    Task<Ranking> CreateAsync(Ranking ranking);
    Task<Ranking> UpdateAsync(Ranking ranking);
    Task DeleteAsync(string id);
}
