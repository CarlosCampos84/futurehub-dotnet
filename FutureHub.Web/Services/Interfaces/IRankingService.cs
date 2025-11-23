using FutureHub.Web.Models.DTOs;

namespace FutureHub.Web.Services.Interfaces;

public interface IRankingService
{
    Task<RankingDTO?> GetByIdAsync(string id);
    Task<IEnumerable<RankingDTO>> GetByPeriodoAsync(string periodo);
    Task<IEnumerable<RankingDetailDTO>> GetTopRankingsAsync(int limit = 10);
    Task AtualizarRankingAsync(string usuarioId);
}
