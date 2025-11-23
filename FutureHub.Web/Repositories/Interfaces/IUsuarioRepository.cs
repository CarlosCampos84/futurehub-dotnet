using FutureHub.Web.Models;

namespace FutureHub.Web.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(string id);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<Usuario> UpdateAsync(Usuario usuario);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> EmailExistsAsync(string email);
}
