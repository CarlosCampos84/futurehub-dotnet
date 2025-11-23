using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FutureHub.Web.Services;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Models;
using FutureHub.Web.Models.Configuration;
using FutureHub.Web.Models.DTOs;

namespace FutureHub.Testes;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_DeveLancarExcecao_QuandoEmailJaExiste()
    {
        // Arrange
        var mockUsuarioRepo = new Mock<IUsuarioRepository>();
        var mockAreaRepo = new Mock<IAreaRepository>();
        var mockLogger = new Mock<ILogger<AuthService>>();
        var jwtSettings = Options.Create(new JwtSettings
        {
            SecretKey = "chave-secreta-super-segura-com-pelo-menos-32-caracteres-minimo",
            Issuer = "FutureHub",
            Audience = "FutureHub-Users",
            ExpirationMinutes = 60
        });

        mockUsuarioRepo.Setup(r => r.EmailExistsAsync("joao@test.com"))
            .ReturnsAsync(true);

        var service = new AuthService(
            mockUsuarioRepo.Object,
            mockAreaRepo.Object,
            jwtSettings,
            mockLogger.Object
        );

        var registerDTO = new RegisterDTO
        {
            Nome = "João Silva",
            Email = "joao@test.com",
            Senha = "senha123"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.RegisterAsync(registerDTO)
        );

        Assert.Equal("Email já cadastrado", exception.Message);
        mockUsuarioRepo.Verify(r => r.CreateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_DeveLancarExcecao_QuandoSenhaIncorreta()
    {
        // Arrange
        var mockUsuarioRepo = new Mock<IUsuarioRepository>();
        var mockAreaRepo = new Mock<IAreaRepository>();
        var mockLogger = new Mock<ILogger<AuthService>>();
        var jwtSettings = Options.Create(new JwtSettings
        {
            SecretKey = "chave-secreta-super-segura-com-pelo-menos-32-caracteres-minimo",
            Issuer = "FutureHub",
            Audience = "FutureHub-Users",
            ExpirationMinutes = 60
        });

        var usuario = new Usuario
        {
            Id = "user-123",
            Nome = "Maria Santos",
            Email = "maria@test.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senhaCorreta123"),
            Role = "ROLE_USER"
        };

        mockUsuarioRepo.Setup(r => r.GetByEmailAsync("maria@test.com"))
            .ReturnsAsync(usuario);

        var service = new AuthService(
            mockUsuarioRepo.Object,
            mockAreaRepo.Object,
            jwtSettings,
            mockLogger.Object
        );

        var loginDTO = new LoginDTO
        {
            Email = "maria@test.com",
            Senha = "senhaErrada123" // Senha incorreta
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => service.LoginAsync(loginDTO)
        );

        Assert.Equal("Email ou senha inválidos", exception.Message);
    }
}
