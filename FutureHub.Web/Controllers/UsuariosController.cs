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
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    /// <summary>
    /// Buscar usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDTO>> GetById(string id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
            return NotFound(new { message = "Usuário não encontrado" });

        return Ok(usuario);
    }

    /// <summary>
    /// Listar todos os usuários (paginado)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "ROLE_ADMIN")]
    [ProducesResponseType(typeof(PagedResult<UsuarioDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<UsuarioDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var todosUsuarios = await _usuarioService.GetAllAsync();
        var totalCount = todosUsuarios.Count();
        
        var usuarios = todosUsuarios
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = HateoasLinks.CreatePagedResult(
            usuarios,
            page,
            pageSize,
            totalCount,
            Request,
            "usuarios"
        );

        return Ok(result);
    }

    /// <summary>
    /// Atualizar usuário
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDTO>> Update(string id, [FromBody] UsuarioUpdateDTO dto)
    {
        try
        {
            var usuario = await _usuarioService.UpdateAsync(id, dto);
            return Ok(usuario);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletar usuário
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
