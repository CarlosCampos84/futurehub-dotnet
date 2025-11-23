using FutureHub.Web.Models;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Services.Interfaces;

namespace FutureHub.Web.Services;

public class IdeiaService : IIdeiaService
{
    private readonly IIdeiaRepository _ideiaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRankingService _rankingService;
    private readonly ILogger<IdeiaService> _logger;

    public IdeiaService(
        IIdeiaRepository ideiaRepository,
        IUsuarioRepository usuarioRepository,
        IRankingService rankingService,
        ILogger<IdeiaService> logger)
    {
        _ideiaRepository = ideiaRepository;
        _usuarioRepository = usuarioRepository;
        _rankingService = rankingService;
        _logger = logger;
    }

    public async Task<IdeiaDTO?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Buscando ideia por ID: {Id}", id);
        var ideia = await _ideiaRepository.GetByIdAsync(id);
        return ideia != null ? IdeiaDTO.FromEntity(ideia) : null;
    }

    public async Task<IEnumerable<IdeiaDTO>> GetAllAsync()
    {
        _logger.LogInformation("Listando todas as ideias");
        var ideias = await _ideiaRepository.GetAllAsync();
        return ideias.Select(IdeiaDTO.FromEntity);
    }

    public async Task<IEnumerable<IdeiaDTO>> GetByAutorIdAsync(string autorId)
    {
        _logger.LogInformation("Buscando ideias do autor: {AutorId}", autorId);
        var ideias = await _ideiaRepository.GetByAutorIdAsync(autorId);
        return ideias.Select(IdeiaDTO.FromEntity);
    }

    public async Task<IEnumerable<IdeiaDTO>> SearchByTituloAsync(string query)
    {
        _logger.LogInformation("Buscando ideias por título: {Query}", query);
        var ideias = await _ideiaRepository.SearchByTituloAsync(query);
        return ideias.Select(IdeiaDTO.FromEntity);
    }

    public async Task<IEnumerable<IdeiaDTO>> GetByAreaIdAsync(string areaId)
    {
        _logger.LogInformation("Buscando ideias por área: {AreaId}", areaId);
        var ideias = await _ideiaRepository.GetByAreaIdAsync(areaId);
        return ideias.Select(IdeiaDTO.FromEntity);
    }

    public async Task<IdeiaDTO> CreateAsync(IdeiaCreateDTO dto)
    {
        throw new NotImplementedException("Use CreateAsync(dto, autorId) extraindo o autorId do JWT");
    }

    public async Task<IdeiaDTO> CreateAsync(IdeiaCreateDTO dto, string autorId)
    {
        _logger.LogInformation("Criando nova ideia para usuário: {UsuarioId}", autorId);

        var ideia = new Ideia
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            AutorId = autorId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _ideiaRepository.CreateAsync(ideia);
        _logger.LogInformation("Ideia criada com sucesso: {Id}", created.Id);

        // Atualizar ranking do usuário
        await _rankingService.AtualizarRankingAsync(autorId);

        // Recarregar com navegação
        var ideiaCompleta = await _ideiaRepository.GetByIdAsync(created.Id);
        return IdeiaDTO.FromEntity(ideiaCompleta!);
    }

    public async Task<IdeiaDTO> UpdateAsync(string id, IdeiaUpdateDTO dto)
    {
        _logger.LogInformation("Atualizando ideia: {Id}", id);

        var ideia = await _ideiaRepository.GetByIdAsync(id);
        if (ideia == null)
        {
            _logger.LogWarning("Ideia não encontrada para atualização: {Id}", id);
            throw new KeyNotFoundException("Ideia não encontrada");
        }

        if (!string.IsNullOrEmpty(dto.Titulo))
            ideia.Titulo = dto.Titulo;

        if (!string.IsNullOrEmpty(dto.Descricao))
            ideia.Descricao = dto.Descricao;

        var updated = await _ideiaRepository.UpdateAsync(ideia);
        _logger.LogInformation("Ideia atualizada com sucesso: {Id}", id);

        return IdeiaDTO.FromEntity(updated);
    }

    public async Task DeleteAsync(string id)
    {
        _logger.LogInformation("Deletando ideia: {Id}", id);

        if (!await _ideiaRepository.ExistsAsync(id))
        {
            _logger.LogWarning("Ideia não encontrada para deleção: {Id}", id);
            throw new KeyNotFoundException("Ideia não encontrada");
        }

        await _ideiaRepository.DeleteAsync(id);
        _logger.LogInformation("Ideia deletada com sucesso: {Id}", id);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _ideiaRepository.GetTotalCountAsync();
    }
}
