using Microsoft.AspNetCore.Mvc;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Services.Interfaces;

namespace FutureHub.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Autenticar usuário e obter token JWT
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginDTO dto)
    {
        try
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Falha no login para: {Email}", dto.Email);
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Registrar novo usuário e obter token JWT
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponse>> Register([FromBody] RegisterDTO dto)
    {
        try
        {
            var response = await _authService.RegisterAsync(dto);
            return CreatedAtAction(nameof(Register), response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro no registro para: {Email}", dto.Email);
            return BadRequest(new { message = ex.Message });
        }
    }
}
