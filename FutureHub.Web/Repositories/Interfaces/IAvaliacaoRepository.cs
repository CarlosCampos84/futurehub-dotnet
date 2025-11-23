using FutureHub.Web.Models;

namespace FutureHub.Web.Repositories.Interfaces;

public interface IAvaliacaoRepository
{
    Task<Avaliacao?> GetByIdAsync(string id);
    Task<IEnumerable<Avaliacao>> GetByIdeiaIdAsync(string ideiaId);
    Task<Avaliacao> CreateAsync(Avaliacao avaliacao);
    Task DeleteAsync(string id);
    Task<double> GetMediaNotasByIdeiaIdAsync(string ideiaId);
    Task<int> GetTotalAvaliacoesByIdeiaIdAsync(string ideiaId);
}
