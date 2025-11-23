using FutureHub.Web.Models.DTOs;

namespace FutureHub.Web.Services.Interfaces;

public interface IIdeiaService
{
    Task<IdeiaDTO?> GetByIdAsync(string id);
    Task<IEnumerable<IdeiaDTO>> GetAllAsync();
    Task<IEnumerable<IdeiaDTO>> GetByAutorIdAsync(string autorId);
    Task<IEnumerable<IdeiaDTO>> SearchByTituloAsync(string query);
    Task<IEnumerable<IdeiaDTO>> GetByAreaIdAsync(string areaId);
    Task<IdeiaDTO> CreateAsync(IdeiaCreateDTO dto);
    Task<IdeiaDTO> CreateAsync(IdeiaCreateDTO dto, string autorId);
    Task<IdeiaDTO> UpdateAsync(string id, IdeiaUpdateDTO dto);
    Task DeleteAsync(string id);
    Task<int> GetTotalCountAsync();
}
