using FutureHub.Web.Models.DTOs;

namespace FutureHub.Web.Services.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDTO?> GetByIdAsync(string id);
    Task<UsuarioDTO?> GetByEmailAsync(string email);
    Task<IEnumerable<UsuarioDTO>> GetAllAsync();
    Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO dto);
    Task<UsuarioDTO> UpdateAsync(string id, UsuarioUpdateDTO dto);
    Task DeleteAsync(string id);
}
