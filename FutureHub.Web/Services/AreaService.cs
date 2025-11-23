using FutureHub.Web.Models;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Services.Interfaces;

namespace FutureHub.Web.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly ILogger<AreaService> _logger;

    public AreaService(IAreaRepository areaRepository, ILogger<AreaService> logger)
    {
        _areaRepository = areaRepository;
        _logger = logger;
    }

    public async Task<AreaDTO?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Buscando área por ID: {Id}", id);
        var area = await _areaRepository.GetByIdAsync(id);
        return area != null ? AreaDTO.FromEntity(area) : null;
    }

    public async Task<IEnumerable<AreaDTO>> GetAllAsync()
    {
        _logger.LogInformation("Listando todas as áreas");
        var areas = await _areaRepository.GetAllAsync();
        return areas.Select(AreaDTO.FromEntity);
    }

    public async Task<AreaDTO> CreateAsync(AreaCreateDTO dto)
    {
        _logger.LogInformation("Criando nova área: {Nome}", dto.Nome);

        var area = new Area
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao
        };

        var created = await _areaRepository.CreateAsync(area);
        _logger.LogInformation("Área criada com sucesso: {Id}", created.Id);

        return AreaDTO.FromEntity(created);
    }

    public async Task<AreaDTO> UpdateAsync(string id, AreaUpdateDTO dto)
    {
        _logger.LogInformation("Atualizando área: {Id}", id);

        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null)
        {
            _logger.LogWarning("Área não encontrada para atualização: {Id}", id);
            throw new KeyNotFoundException("Área não encontrada");
        }

        if (!string.IsNullOrEmpty(dto.Nome))
            area.Nome = dto.Nome;

        if (!string.IsNullOrEmpty(dto.Descricao))
            area.Descricao = dto.Descricao;

        var updated = await _areaRepository.UpdateAsync(area);
        _logger.LogInformation("Área atualizada com sucesso: {Id}", id);

        return AreaDTO.FromEntity(updated);
    }

    public async Task DeleteAsync(string id)
    {
        _logger.LogInformation("Deletando área: {Id}", id);

        if (!await _areaRepository.ExistsAsync(id))
        {
            _logger.LogWarning("Área não encontrada para deleção: {Id}", id);
            throw new KeyNotFoundException("Área não encontrada");
        }

        await _areaRepository.DeleteAsync(id);
        _logger.LogInformation("Área deletada com sucesso: {Id}", id);
    }
}
