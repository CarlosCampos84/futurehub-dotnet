using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Services.Interfaces;
using System.Security.Claims;
using FutureHub.Web.Helpers;
using FutureHub.Web.Models.Pagination;

namespace FutureHub.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AvaliacoesController : ControllerBase
{
    private readonly IAvaliacaoService _avaliacaoService;
    private readonly ILogger<AvaliacoesController> _logger;

    public AvaliacoesController(IAvaliacaoService avaliacaoService, ILogger<AvaliacoesController> logger)
    {
        _avaliacaoService = avaliacaoService;
        _logger = logger;
    }

    /// <summary>
    /// Criar nova avaliação
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(AvaliacaoDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AvaliacaoDTO>> Create([FromBody] AvaliacaoCreateDTO dto)
    {
        try
        {
            // Extrair ID do usuário autenticado do JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Usuário não autenticado" });

            var avaliacao = await _avaliacaoService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = avaliacao.Id }, avaliacao);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar avaliação");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Buscar avaliação por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AvaliacaoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AvaliacaoDTO>> GetById(string id)
    {
        var avaliacao = await _avaliacaoService.GetByIdAsync(id);
        if (avaliacao == null)
            return NotFound(new { message = "Avaliação não encontrada" });

        return Ok(avaliacao);
    }

    /// <summary>
    /// Listar avaliações de uma ideia (paginado)
    /// </summary>
    [HttpGet("ideia/{ideiaId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<AvaliacaoDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<AvaliacaoDTO>>> GetByIdeiaId(
        string ideiaId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var todasAvaliacoes = await _avaliacaoService.GetByIdeiaIdAsync(ideiaId);
        var totalCount = todasAvaliacoes.Count();
        
        var avaliacoes = todasAvaliacoes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = HateoasLinks.CreatePagedResult(
            avaliacoes,
            page,
            pageSize,
            totalCount,
            Request,
            "avaliacoes"
        );

        return Ok(result);
    }

    /// <summary>
    /// Deletar avaliação
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _avaliacaoService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
