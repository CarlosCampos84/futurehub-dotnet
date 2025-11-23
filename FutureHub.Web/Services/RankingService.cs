using FutureHub.Web.Models;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Services.Interfaces;

namespace FutureHub.Web.Services;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _rankingRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IIdeiaRepository _ideiaRepository;
    private readonly ILogger<RankingService> _logger;

    public RankingService(
        IRankingRepository rankingRepository,
        IUsuarioRepository usuarioRepository,
        IIdeiaRepository ideiaRepository,
        ILogger<RankingService> logger)
    {
        _rankingRepository = rankingRepository;
        _usuarioRepository = usuarioRepository;
        _ideiaRepository = ideiaRepository;
        _logger = logger;
    }

    public async Task<RankingDTO?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Buscando ranking por ID: {Id}", id);
        var ranking = await _rankingRepository.GetByIdAsync(id);
        return ranking != null ? RankingDTO.FromEntity(ranking) : null;
    }

    public async Task<IEnumerable<RankingDTO>> GetByPeriodoAsync(string periodo)
    {
        _logger.LogInformation("Buscando rankings do período: {Periodo}", periodo);
        var rankings = await _rankingRepository.GetByPeriodoAsync(periodo);
        return rankings.Select(RankingDTO.FromEntity);
    }

    public async Task<IEnumerable<RankingDetailDTO>> GetTopRankingsAsync(int limit = 10)
    {
        _logger.LogInformation("Buscando top {Limit} rankings", limit);
        var rankings = await _rankingRepository.GetTopRankingsAsync(limit);

        var result = new List<RankingDetailDTO>();

        foreach (var ranking in rankings)
        {
            var ideias = await _ideiaRepository.GetByAutorIdAsync(ranking.UsuarioId);
            var ideiasLista = ideias.ToList();

            var dto = new RankingDetailDTO
            {
                UsuarioId = ranking.UsuarioId,
                UsuarioNome = ranking.Usuario?.Nome ?? string.Empty,
                PontuacaoTotal = ranking.PontuacaoTotal,
                IdeiasPublicadas = ideiasLista.Count,
                MediaAvaliacoes = ideiasLista.Any() ? ideiasLista.Average(i => i.MediaNotas) : 0.0
            };

            result.Add(dto);
        }

        return result;
    }

    public async Task AtualizarRankingAsync(string usuarioId)
    {
        _logger.LogInformation("Atualizando ranking do usuário: {UsuarioId}", usuarioId);

        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        if (usuario == null)
        {
            _logger.LogWarning("Usuário não encontrado: {UsuarioId}", usuarioId);
            return;
        }

        var periodoAtual = DateTime.Now.ToString("yyyy-MM");
        var ideias = await _ideiaRepository.GetByAutorIdAsync(usuarioId);
        var ideiasLista = ideias.ToList();

        // Calcular pontuação: 10 pontos por ideia + (média de avaliações * 5)
        var pontuacao = ideiasLista.Count * 10;
        foreach (var ideia in ideiasLista)
        {
            pontuacao += (int)(ideia.MediaNotas * 5);
        }

        // Verificar se já existe ranking para este período
        var rankingExistente = await _rankingRepository.GetByUsuarioAndPeriodoAsync(usuarioId, periodoAtual);

        if (rankingExistente != null)
        {
            rankingExistente.PontuacaoTotal = pontuacao;
            await _rankingRepository.UpdateAsync(rankingExistente);
            _logger.LogInformation("Ranking atualizado: {RankingId}", rankingExistente.Id);
        }
        else
        {
            var novoRanking = new Ranking
            {
                UsuarioId = usuarioId,
                PontuacaoTotal = pontuacao,
                Periodo = periodoAtual
            };

            await _rankingRepository.CreateAsync(novoRanking);
            _logger.LogInformation("Novo ranking criado para usuário: {UsuarioId}", usuarioId);
        }

        // Atualizar pontos do usuário
        usuario.Pontos = pontuacao;
        await _usuarioRepository.UpdateAsync(usuario);
    }
}
