using Microsoft.EntityFrameworkCore;
using FutureHub.Web.Data;
using FutureHub.Web.Models;
using FutureHub.Web.Repositories.Interfaces;

namespace FutureHub.Web.Repositories;

public class RankingRepository : IRankingRepository
{
    private readonly OracleDbContext _context;

    public RankingRepository(OracleDbContext context)
    {
        _context = context;
    }

    public async Task<Ranking?> GetByIdAsync(string id)
    {
        return await _context.Rankings
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Ranking?> GetByUsuarioAndPeriodoAsync(string usuarioId, string periodo)
    {
        return await _context.Rankings
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.Periodo == periodo);
    }

    public async Task<IEnumerable<Ranking>> GetByPeriodoAsync(string periodo)
    {
        return await _context.Rankings
            .Include(r => r.Usuario)
            .Where(r => r.Periodo == periodo)
            .OrderByDescending(r => r.PontuacaoTotal)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ranking>> GetTopRankingsAsync(int limit = 10)
    {
        var periodoAtual = DateTime.Now.ToString("yyyy-MM");
        
        return await _context.Rankings
            .Include(r => r.Usuario)
            .Where(r => r.Periodo == periodoAtual)
            .OrderByDescending(r => r.PontuacaoTotal)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Ranking> CreateAsync(Ranking ranking)
    {
        _context.Rankings.Add(ranking);
        await _context.SaveChangesAsync();
        return ranking;
    }

    public async Task<Ranking> UpdateAsync(Ranking ranking)
    {
        _context.Rankings.Update(ranking);
        await _context.SaveChangesAsync();
        return ranking;
    }

    public async Task DeleteAsync(string id)
    {
        var ranking = await _context.Rankings.FindAsync(id);
        if (ranking != null)
        {
            _context.Rankings.Remove(ranking);
            await _context.SaveChangesAsync();
        }
    }
}
