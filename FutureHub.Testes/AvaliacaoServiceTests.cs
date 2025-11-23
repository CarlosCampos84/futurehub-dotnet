using Moq;
using Microsoft.Extensions.Logging;
using FutureHub.Web.Services;
using FutureHub.Web.Services.Interfaces;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Models;
using FutureHub.Web.Models.DTOs;

namespace FutureHub.Testes;

/// <summary>
/// Testes unitários para validação de regras de negócio de Avaliações
/// </summary>
public class AvaliacaoServiceTests
{
    [Fact]
    public async Task CreateAsync_DeveLancarExcecao_QuandoIdeiaInexistente()
    {
        // Arrange - Preparar mocks e dados
        var mockAvaliacaoRepo = new Mock<IAvaliacaoRepository>();
        var mockIdeiaRepo = new Mock<IIdeiaRepository>();
        var mockRankingService = new Mock<IRankingService>();
        var mockLogger = new Mock<ILogger<AvaliacaoService>>();

        // Simular que a ideia não existe
        mockIdeiaRepo.Setup(r => r.GetByIdAsync("ideia-inexistente"))
            .ReturnsAsync((Ideia?)null);

        var service = new AvaliacaoService(
            mockAvaliacaoRepo.Object,
            mockIdeiaRepo.Object,
            mockRankingService.Object,
            mockLogger.Object
        );

        var createDTO = new AvaliacaoCreateDTO
        {
            IdeiaId = "ideia-inexistente",
            Nota = 5
        };

        // Act & Assert - Executar e verificar exceção
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(createDTO, "user-123")
        );

        Assert.Equal("Ideia não encontrada", exception.Message);
        mockAvaliacaoRepo.Verify(r => r.CreateAsync(It.IsAny<Avaliacao>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_DeveAtualizarMediaETotal_QuandoAvaliacaoValida()
    {
        // Arrange
        var mockAvaliacaoRepo = new Mock<IAvaliacaoRepository>();
        var mockIdeiaRepo = new Mock<IIdeiaRepository>();
        var mockRankingService = new Mock<IRankingService>();
        var mockLogger = new Mock<ILogger<AvaliacaoService>>();

        var ideiaId = "ideia-123";
        var autorId = "autor-456";
        var ideia = new Ideia
        {
            Id = ideiaId,
            AutorId = autorId,
            Titulo = "Ideia Sustentável",
            MediaNotas = 4.0,
            TotalAvaliacoes = 2,
            Autor = new Usuario { Id = autorId, Nome = "Autor", Email = "autor@test.com", SenhaHash = "hash" }
        };

        var avaliacoesExistentes = new List<Avaliacao>
        {
            new Avaliacao { Id = "av1", IdeiaId = ideiaId, Nota = 4 },
            new Avaliacao { Id = "av2", IdeiaId = ideiaId, Nota = 4 }
        };

        mockIdeiaRepo.Setup(r => r.GetByIdAsync(ideiaId))
            .ReturnsAsync(ideia);
        mockAvaliacaoRepo.Setup(r => r.CreateAsync(It.IsAny<Avaliacao>()))
            .ReturnsAsync((Avaliacao av) => av);
        mockAvaliacaoRepo.Setup(r => r.GetByIdeiaIdAsync(ideiaId))
            .ReturnsAsync(avaliacoesExistentes);

        var service = new AvaliacaoService(
            mockAvaliacaoRepo.Object,
            mockIdeiaRepo.Object,
            mockRankingService.Object,
            mockLogger.Object
        );

        var createDTO = new AvaliacaoCreateDTO
        {
            IdeiaId = ideiaId,
            Nota = 5 // Nova avaliação com nota 5
        };

        // Act
        var result = await service.CreateAsync(createDTO, "user-789");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Nota);
        
        // Verificar se a avaliação foi criada
        mockAvaliacaoRepo.Verify(r => r.CreateAsync(It.IsAny<Avaliacao>()), Times.Once);
        
        // Verificar que a ideia foi atualizada (média e total recalculados)
        mockIdeiaRepo.Verify(r => r.UpdateAsync(It.IsAny<Ideia>()), Times.Once);
    }
}

