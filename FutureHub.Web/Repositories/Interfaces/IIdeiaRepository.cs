using FutureHub.Web.Models;

namespace FutureHub.Web.Repositories.Interfaces;

public interface IIdeiaRepository
{
    Task<Ideia?> GetByIdAsync(string id);
    Task<IEnumerable<Ideia>> GetAllAsync();
    Task<IEnumerable<Ideia>> GetByAutorIdAsync(string autorId);
    Task<IEnumerable<Ideia>> SearchByTituloAsync(string query);
    Task<IEnumerable<Ideia>> GetByAreaIdAsync(string areaId);
    Task<Ideia> CreateAsync(Ideia ideia);
    Task<Ideia> UpdateAsync(Ideia ideia);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<int> GetTotalCountAsync();
}
