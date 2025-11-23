using FutureHub.Web.Models;

namespace FutureHub.Web.Repositories.Interfaces;

public interface IAreaRepository
{
    Task<Area?> GetByIdAsync(string id);
    Task<Area?> GetByNomeAsync(string nome);
    Task<IEnumerable<Area>> GetAllAsync();
    Task<Area> CreateAsync(Area area);
    Task<Area> UpdateAsync(Area area);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}
