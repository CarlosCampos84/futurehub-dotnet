using Moq;
using Microsoft.Extensions.Logging;
using FutureHub.Web.Services;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Models;

namespace FutureHub.Testes;

public class RankingServiceTests
{
    [Fact]
    public async Task AtualizarRankingAsync_DeveCacularPontuacaoCorretamente_ComUmaIdeiaSemAvaliacoes()
    {
        // Arrange - Preparar dados de teste
        var mockRankingRepo = new Mock<IRankingRepository>();
        var mockUsuarioRepo = new Mock<IUsuarioRepository>();
        var mockIdeiaRepo = new Mock<IIdeiaRepository>();
        var mockLogger = new Mock<ILogger<RankingService>>();

        var usuarioId = "user-123";
        var usuario = new Usuario
        {
            Id = usuarioId,
            Nome = "João Silva",
            Email = "joao@test.com",
            SenhaHash = "hash",
            Pontos = 0
        };

        var ideias = new List<Ideia>
        {
            new Ideia
            {
                Id = "ideia-1",
                AutorId = usuarioId,
                Titulo = "Ideia Sustentável",
                MediaNotas = 0, // Sem avaliações
                TotalAvaliacoes = 0
            }
        };

        mockUsuarioRepo.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        mockIdeiaRepo.Setup(r => r.GetByAutorIdAsync(usuarioId))
            .ReturnsAsync(ideias);
        mockRankingRepo.Setup(r => r.GetByUsuarioAndPeriodoAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((Ranking?)null);

        var service = new RankingService(
            mockRankingRepo.Object,
            mockUsuarioRepo.Object,
            mockIdeiaRepo.Object,
            mockLogger.Object
        );

        // Act - Executar a ação
        await service.AtualizarRankingAsync(usuarioId);

        // Assert - Verificar resultado esperado
        // Regra: 10 pontos por ideia + (média * 5)
        // Esperado: 1 ideia * 10 + (0 * 5) = 10 pontos
        mockUsuarioRepo.Verify(r => r.UpdateAsync(It.Is<Usuario>(u => u.Pontos == 10)), Times.Once);
        mockRankingRepo.Verify(r => r.CreateAsync(It.Is<Ranking>(rk => rk.PontuacaoTotal == 10)), Times.Once);
    }

    [Fact]
    public async Task AtualizarRankingAsync_DeveCacularPontuacaoCorretamente_ComMultiplasIdeiasAvaliadas()
    {
        // Arrange
        var mockRankingRepo = new Mock<IRankingRepository>();
        var mockUsuarioRepo = new Mock<IUsuarioRepository>();
        var mockIdeiaRepo = new Mock<IIdeiaRepository>();
        var mockLogger = new Mock<ILogger<RankingService>>();

        var usuarioId = "user-456";
        var usuario = new Usuario
        {
            Id = usuarioId,
            Nome = "Maria Santos",
            Email = "maria@test.com",
            SenhaHash = "hash",
            Pontos = 0
        };

        var ideias = new List<Ideia>
        {
            new Ideia { Id = "i1", AutorId = usuarioId, Titulo = "Ideia 1", MediaNotas = 4.5, TotalAvaliacoes = 10 },
            new Ideia { Id = "i2", AutorId = usuarioId, Titulo = "Ideia 2", MediaNotas = 3.0, TotalAvaliacoes = 5 },
            new Ideia { Id = "i3", AutorId = usuarioId, Titulo = "Ideia 3", MediaNotas = 5.0, TotalAvaliacoes = 20 }
        };

        mockUsuarioRepo.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        mockIdeiaRepo.Setup(r => r.GetByAutorIdAsync(usuarioId))
            .ReturnsAsync(ideias);
        mockRankingRepo.Setup(r => r.GetByUsuarioAndPeriodoAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((Ranking?)null);

        var service = new RankingService(
            mockRankingRepo.Object,
            mockUsuarioRepo.Object,
            mockIdeiaRepo.Object,
            mockLogger.Object
        );

        // Act
        await service.AtualizarRankingAsync(usuarioId);

        // Assert
        // Regra: 10 pontos por ideia + (média * 5)
        // 3 ideias = 30 pontos base
        // Ideia 1: 4.5 * 5 = 22.5 → 22 (int)
        // Ideia 2: 3.0 * 5 = 15.0 → 15 (int)
        // Ideia 3: 5.0 * 5 = 25.0 → 25 (int)
        // Total: 30 + 22 + 15 + 25 = 92 pontos
        mockUsuarioRepo.Verify(r => r.UpdateAsync(It.Is<Usuario>(u => u.Pontos == 92)), Times.Once);
        mockRankingRepo.Verify(r => r.CreateAsync(It.Is<Ranking>(rk => rk.PontuacaoTotal == 92)), Times.Once);
    }

    [Fact]
    public async Task AtualizarRankingAsync_DeveAtualizarRankingExistente_QuandoJaExisteParaPeriodo()
    {
        // Arrange
        var mockRankingRepo = new Mock<IRankingRepository>();
        var mockUsuarioRepo = new Mock<IUsuarioRepository>();
        var mockIdeiaRepo = new Mock<IIdeiaRepository>();
        var mockLogger = new Mock<ILogger<RankingService>>();

        var usuarioId = "user-789";
        var rankingId = "ranking-existing";
        var periodoAtual = DateTime.Now.ToString("yyyy-MM");

        var usuario = new Usuario
        {
            Id = usuarioId,
            Nome = "Carlos Silva",
            Email = "carlos@test.com",
            SenhaHash = "hash",
            Pontos = 50
        };

        var rankingExistente = new Ranking
        {
            Id = rankingId,
            UsuarioId = usuarioId,
            PontuacaoTotal = 50,
            Periodo = periodoAtual
        };

        var ideias = new List<Ideia>
        {
            new Ideia { Id = "i1", AutorId = usuarioId, Titulo = "Nova Ideia", MediaNotas = 4.0, TotalAvaliacoes = 10 }
        };

        mockUsuarioRepo.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        mockIdeiaRepo.Setup(r => r.GetByAutorIdAsync(usuarioId))
            .ReturnsAsync(ideias);
        mockRankingRepo.Setup(r => r.GetByUsuarioAndPeriodoAsync(usuarioId, It.IsAny<string>()))
            .ReturnsAsync(rankingExistente);

        var service = new RankingService(
            mockRankingRepo.Object,
            mockUsuarioRepo.Object,
            mockIdeiaRepo.Object,
            mockLogger.Object
        );

        // Act
        await service.AtualizarRankingAsync(usuarioId);

        // Assert
        // Regra: 10 + (4.0 * 5) = 10 + 20 = 30 pontos
        mockRankingRepo.Verify(r => r.UpdateAsync(It.Is<Ranking>(rk => 
            rk.Id == rankingId && 
            rk.PontuacaoTotal == 30
        )), Times.Once);
        mockRankingRepo.Verify(r => r.CreateAsync(It.IsAny<Ranking>()), Times.Never);
    }
}
