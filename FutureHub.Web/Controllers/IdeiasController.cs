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
public class IdeiasController : ControllerBase
{
    private readonly IIdeiaService _ideiaService;
    private readonly ILogger<IdeiasController> _logger;

    public IdeiasController(IIdeiaService ideiaService, ILogger<IdeiasController> logger)
    {
        _ideiaService = ideiaService;
        _logger = logger;
    }

    /// <summary>
    /// Criar nova ideia
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(IdeiaDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdeiaDTO>> Create([FromBody] IdeiaCreateDTO dto)
    {
        try
        {
            // Extrair ID do usuário autenticado do JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Usuário não autenticado" });

            var ideia = await _ideiaService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = ideia.Id }, ideia);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar ideia");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Listar ideias (paginado, com filtros opcionais)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<IdeiaDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<IdeiaDTO>>> GetAll(
        [FromQuery] string? areaId,
        [FromQuery] string? q,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        IEnumerable<IdeiaDTO> ideias;

        if (!string.IsNullOrEmpty(areaId))
        {
            ideias = await _ideiaService.GetByAreaIdAsync(areaId);
        }
        else if (!string.IsNullOrEmpty(q))
        {
            ideias = await _ideiaService.SearchByTituloAsync(q);
        }
        else
        {
            ideias = await _ideiaService.GetAllAsync();
        }

        var totalCount = ideias.Count();
        
        var ideiasPage = ideias
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = HateoasLinks.CreatePagedResult(
            ideiasPage,
            page,
            pageSize,
            totalCount,
            Request,
            "ideias"
        );

        return Ok(result);
    }

    /// <summary>
    /// Buscar ideia por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IdeiaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdeiaDTO>> GetById(string id)
    {
        var ideia = await _ideiaService.GetByIdAsync(id);
        if (ideia == null)
            return NotFound(new { message = "Ideia não encontrada" });

        return Ok(ideia);
    }

    /// <summary>
    /// Atualizar ideia
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    [ProducesResponseType(typeof(IdeiaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdeiaDTO>> Update(string id, [FromBody] IdeiaUpdateDTO dto)
    {
        try
        {
            var ideia = await _ideiaService.UpdateAsync(id, dto);
            return Ok(ideia);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletar ideia
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _ideiaService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
