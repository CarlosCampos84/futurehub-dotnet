using FutureHub.Web.Models;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Services.Interfaces;

namespace FutureHub.Web.Services;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepository;
    private readonly IIdeiaRepository _ideiaRepository;
    private readonly IRankingService _rankingService;
    private readonly ILogger<AvaliacaoService> _logger;

    public AvaliacaoService(
        IAvaliacaoRepository avaliacaoRepository,
        IIdeiaRepository ideiaRepository,
        IRankingService rankingService,
        ILogger<AvaliacaoService> logger)
    {
        _avaliacaoRepository = avaliacaoRepository;
        _ideiaRepository = ideiaRepository;
        _rankingService = rankingService;
        _logger = logger;
    }

    public async Task<AvaliacaoDTO?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Buscando avaliação por ID: {Id}", id);
        var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
        return avaliacao != null ? AvaliacaoDTO.FromEntity(avaliacao) : null;
    }

    public async Task<IEnumerable<AvaliacaoDTO>> GetByIdeiaIdAsync(string ideiaId)
    {
        _logger.LogInformation("Buscando avaliações da ideia: {IdeiaId}", ideiaId);
        var avaliacoes = await _avaliacaoRepository.GetByIdeiaIdAsync(ideiaId);
        return avaliacoes.Select(AvaliacaoDTO.FromEntity);
    }

    public async Task<AvaliacaoDTO> CreateAsync(AvaliacaoCreateDTO dto)
    {
        throw new NotImplementedException("Use CreateAsync(dto, userId) extraindo o userId do JWT");
    }

    public async Task<AvaliacaoDTO> CreateAsync(AvaliacaoCreateDTO dto, string userId)
    {
        _logger.LogInformation("Criando avaliação para ideia: {IdeiaId} pelo usuário: {UserId}", dto.IdeiaId, userId);

        // Validar se ideia existe e carregar com autor
        var ideia = await _ideiaRepository.GetByIdAsync(dto.IdeiaId);
        if (ideia == null)
        {
            _logger.LogWarning("Ideia não encontrada: {IdeiaId}", dto.IdeiaId);
            throw new InvalidOperationException("Ideia não encontrada");
        }

        var avaliacao = new Avaliacao
        {
            IdeiaId = dto.IdeiaId,
            Nota = dto.Nota,
            DataAvaliacao = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _avaliacaoRepository.CreateAsync(avaliacao);
        _logger.LogInformation("Avaliação criada com sucesso: {Id}", created.Id);

        // Atualizar média e total de avaliações da ideia
        await AtualizarEstatisticasIdeiaAsync(dto.IdeiaId);

        // Atualizar ranking do autor da ideia (assíncrono)
        _ = Task.Run(async () =>
        {
            try
            {
                await _rankingService.AtualizarRankingAsync(ideia.AutorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar ranking do usuário: {UsuarioId}", ideia.AutorId);
            }
        });

        return AvaliacaoDTO.FromEntity(created);
    }

    public async Task DeleteAsync(string id)
    {
        _logger.LogInformation("Deletando avaliação: {Id}", id);

        var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
        if (avaliacao == null)
        {
            _logger.LogWarning("Avaliação não encontrada para deleção: {Id}", id);
            throw new KeyNotFoundException("Avaliação não encontrada");
        }

        var ideiaId = avaliacao.IdeiaId;
        await _avaliacaoRepository.DeleteAsync(id);
        _logger.LogInformation("Avaliação deletada com sucesso: {Id}", id);

        // Atualizar estatísticas da ideia
        await AtualizarEstatisticasIdeiaAsync(ideiaId);
    }

    private async Task AtualizarEstatisticasIdeiaAsync(string ideiaId)
    {
        var ideia = await _ideiaRepository.GetByIdAsync(ideiaId);
        if (ideia == null) return;

        var mediaNotas = await _avaliacaoRepository.GetMediaNotasByIdeiaIdAsync(ideiaId);
        var totalAvaliacoes = await _avaliacaoRepository.GetTotalAvaliacoesByIdeiaIdAsync(ideiaId);

        ideia.MediaNotas = mediaNotas;
        ideia.TotalAvaliacoes = totalAvaliacoes;

        await _ideiaRepository.UpdateAsync(ideia);
        _logger.LogInformation("Estatísticas da ideia atualizadas: {IdeiaId}", ideiaId);
    }
}
