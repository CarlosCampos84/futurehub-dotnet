using FutureHub.Web.Models.DTOs;

namespace FutureHub.Web.Services.Interfaces;

public interface IAreaService
{
    Task<AreaDTO?> GetByIdAsync(string id);
    Task<IEnumerable<AreaDTO>> GetAllAsync();
    Task<AreaDTO> CreateAsync(AreaCreateDTO dto);
    Task<AreaDTO> UpdateAsync(string id, AreaUpdateDTO dto);
    Task DeleteAsync(string id);
}
