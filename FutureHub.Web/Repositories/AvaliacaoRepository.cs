using Microsoft.EntityFrameworkCore;
using FutureHub.Web.Data;
using FutureHub.Web.Models;
using FutureHub.Web.Repositories.Interfaces;

namespace FutureHub.Web.Repositories;

public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly OracleDbContext _context;

    public AvaliacaoRepository(OracleDbContext context)
    {
        _context = context;
    }

    public async Task<Avaliacao?> GetByIdAsync(string id)
    {
        return await _context.Avaliacoes
            .Include(a => a.Ideia)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Avaliacao>> GetByIdeiaIdAsync(string ideiaId)
    {
        return await _context.Avaliacoes
            .Where(a => a.IdeiaId == ideiaId)
            .OrderByDescending(a => a.DataAvaliacao)
            .ToListAsync();
    }

    public async Task<Avaliacao> CreateAsync(Avaliacao avaliacao)
    {
        _context.Avaliacoes.Add(avaliacao);
        await _context.SaveChangesAsync();
        return avaliacao;
    }

    public async Task DeleteAsync(string id)
    {
        var avaliacao = await _context.Avaliacoes.FindAsync(id);
        if (avaliacao != null)
        {
            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<double> GetMediaNotasByIdeiaIdAsync(string ideiaId)
    {
        var avaliacoes = await _context.Avaliacoes
            .Where(a => a.IdeiaId == ideiaId)
            .ToListAsync();

        if (!avaliacoes.Any())
            return 0.0;

        return avaliacoes.Average(a => a.Nota);
    }

    public async Task<int> GetTotalAvaliacoesByIdeiaIdAsync(string ideiaId)
    {
        return await _context.Avaliacoes
            .CountAsync(a => a.IdeiaId == ideiaId);
    }
}
