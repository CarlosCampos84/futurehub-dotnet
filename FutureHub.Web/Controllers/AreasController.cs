using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Services.Interfaces;
using FutureHub.Web.Helpers;
using FutureHub.Web.Models.Pagination;

namespace FutureHub.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AreasController : ControllerBase
{
    private readonly IAreaService _areaService;
    private readonly ILogger<AreasController> _logger;

    public AreasController(IAreaService areaService, ILogger<AreasController> logger)
    {
        _areaService = areaService;
        _logger = logger;
    }

    /// <summary>
    /// Criar nova área
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AreaDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AreaDTO>> Create([FromBody] AreaCreateDTO dto)
    {
        var area = await _areaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = area.Id }, area);
    }

    /// <summary>
    /// Listar todas as áreas (paginado)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<AreaDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<AreaDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var todasAreas = await _areaService.GetAllAsync();
        var totalCount = todasAreas.Count();
        
        var areas = todasAreas
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = HateoasLinks.CreatePagedResult(
            areas,
            page,
            pageSize,
            totalCount,
            Request,
            "areas"
        );

        return Ok(result);
    }

    /// <summary>
    /// Buscar área por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AreaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AreaDTO>> GetById(string id)
    {
        var area = await _areaService.GetByIdAsync(id);
        if (area == null)
            return NotFound(new { message = "Área não encontrada" });

        return Ok(area);
    }

    /// <summary>
    /// Atualizar área
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    [ProducesResponseType(typeof(AreaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AreaDTO>> Update(string id, [FromBody] AreaUpdateDTO dto)
    {
        try
        {
            var area = await _areaService.UpdateAsync(id, dto);
            return Ok(area);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletar área
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _areaService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
