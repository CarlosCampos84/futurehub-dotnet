using FutureHub.Web.Models.DTOs;

namespace FutureHub.Web.Services.Interfaces;

public interface IAvaliacaoService
{
    Task<AvaliacaoDTO?> GetByIdAsync(string id);
    Task<IEnumerable<AvaliacaoDTO>> GetByIdeiaIdAsync(string ideiaId);
    Task<AvaliacaoDTO> CreateAsync(AvaliacaoCreateDTO dto);
    Task<AvaliacaoDTO> CreateAsync(AvaliacaoCreateDTO dto, string userId);
    Task DeleteAsync(string id);
}
