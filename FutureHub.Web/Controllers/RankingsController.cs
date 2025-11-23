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
public class RankingsController : ControllerBase
{
    private readonly IRankingService _rankingService;
    private readonly ILogger<RankingsController> _logger;

    public RankingsController(IRankingService rankingService, ILogger<RankingsController> logger)
    {
        _rankingService = rankingService;
        _logger = logger;
    }

    /// <summary>
    /// Top usuários por pontuação (paginado)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<RankingDetailDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<RankingDetailDTO>>> GetTopRankings(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Buscar top rankings (dobro do necessário para paginação)
        var todosRankings = await _rankingService.GetTopRankingsAsync(page * pageSize * 2);
        var totalCount = todosRankings.Count();
        
        var rankings = todosRankings
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = HateoasLinks.CreatePagedResult(
            rankings,
            page,
            pageSize,
            totalCount,
            Request,
            "rankings"
        );

        return Ok(result);
    }

    /// <summary>
    /// Buscar ranking por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RankingDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RankingDTO>> GetById(string id)
    {
        var ranking = await _rankingService.GetByIdAsync(id);
        if (ranking == null)
            return NotFound(new { message = "Ranking não encontrado" });

        return Ok(ranking);
    }

    /// <summary>
    /// Buscar rankings por período (formato: YYYY-MM, paginado)
    /// </summary>
    [HttpGet("periodo/{periodo}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<RankingDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<RankingDTO>>> GetByPeriodo(
        string periodo,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var todosRankings = await _rankingService.GetByPeriodoAsync(periodo);
        var totalCount = todosRankings.Count();
        
        var rankings = todosRankings
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = HateoasLinks.CreatePagedResult(
            rankings,
            page,
            pageSize,
            totalCount,
            Request,
            "rankings"
        );

        return Ok(result);
    }
}
