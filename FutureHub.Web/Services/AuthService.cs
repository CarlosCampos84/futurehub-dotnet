using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FutureHub.Web.Models;
using FutureHub.Web.Models.Configuration;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Services.Interfaces;

namespace FutureHub.Web.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IAreaRepository areaRepository,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _areaRepository = areaRepository;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<TokenResponse> LoginAsync(LoginDTO dto)
    {
        _logger.LogInformation("Tentativa de login para email: {Email}", dto.Email);

        var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (usuario == null)
        {
            _logger.LogWarning("Usuário não encontrado: {Email}", dto.Email);
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        }

        // Verificar senha
        if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
        {
            _logger.LogWarning("Senha incorreta para usuário: {Email}", dto.Email);
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        }

        var token = GenerateJwtToken(usuario);
        _logger.LogInformation("Login realizado com sucesso: {Email}", dto.Email);

        return new TokenResponse
        {
            Token = token,
            ExpiresIn = _jwtSettings.ExpirationMinutes * 60,
            Usuario = UsuarioDTO.FromEntity(usuario)
        };
    }

    public async Task<TokenResponse> RegisterAsync(RegisterDTO dto)
    {
        _logger.LogInformation("Tentativa de registro para email: {Email}", dto.Email);

        // Verificar se email já existe
        if (await _usuarioRepository.EmailExistsAsync(dto.Email))
        {
            _logger.LogWarning("Email já cadastrado: {Email}", dto.Email);
            throw new InvalidOperationException("Email já cadastrado");
        }

        // Validar área de interesse SOMENTE se fornecida
        if (!string.IsNullOrEmpty(dto.AreaInteresseId))
        {
            if (!await _areaRepository.ExistsAsync(dto.AreaInteresseId))
            {
                _logger.LogWarning("Área de interesse não encontrada: {AreaId}", dto.AreaInteresseId);
                throw new InvalidOperationException("Área de interesse não encontrada");
            }
        }

        // Criar usuário (AreaInteresseId pode ser null)
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            AreaInteresseId = string.IsNullOrEmpty(dto.AreaInteresseId) ? null : dto.AreaInteresseId,
            Role = "ROLE_USER"
        };

        await _usuarioRepository.CreateAsync(usuario);
        _logger.LogInformation("Usuário registrado com sucesso: {Email}", dto.Email);

        // Recarregar com navegação
        usuario = await _usuarioRepository.GetByEmailAsync(usuario.Email);

        var token = GenerateJwtToken(usuario!);

        return new TokenResponse
        {
            Token = token,
            ExpiresIn = _jwtSettings.ExpirationMinutes * 60,
            Usuario = UsuarioDTO.FromEntity(usuario!)
        };
    }

    public string GenerateJwtToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
