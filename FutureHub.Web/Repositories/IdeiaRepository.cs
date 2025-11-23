using Microsoft.EntityFrameworkCore;
using FutureHub.Web.Data;
using FutureHub.Web.Models;
using FutureHub.Web.Repositories.Interfaces;

namespace FutureHub.Web.Repositories;

public class IdeiaRepository : IIdeiaRepository
{
    private readonly OracleDbContext _context;

    public IdeiaRepository(OracleDbContext context)
    {
        _context = context;
    }

    public async Task<Ideia?> GetByIdAsync(string id)
    {
        return await _context.Ideias
            .Include(i => i.Autor)
                .ThenInclude(a => a.AreaInteresse)
            .Include(i => i.Avaliacoes)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Ideia>> GetAllAsync()
    {
        return await _context.Ideias
            .Include(i => i.Autor)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ideia>> GetByAutorIdAsync(string autorId)
    {
        return await _context.Ideias
            .Include(i => i.Autor)
            .Where(i => i.AutorId == autorId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ideia>> SearchByTituloAsync(string query)
    {
        return await _context.Ideias
            .Include(i => i.Autor)
            .Where(i => i.Titulo.Contains(query))
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ideia>> GetByAreaIdAsync(string areaId)
    {
        return await _context.Ideias
            .Include(i => i.Autor)
            .Where(i => i.Autor.AreaInteresseId == areaId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<Ideia> CreateAsync(Ideia ideia)
    {
        _context.Ideias.Add(ideia);
        await _context.SaveChangesAsync();
        return ideia;
    }

    public async Task<Ideia> UpdateAsync(Ideia ideia)
    {
        _context.Ideias.Update(ideia);
        await _context.SaveChangesAsync();
        return ideia;
    }

    public async Task DeleteAsync(string id)
    {
        var ideia = await _context.Ideias.FindAsync(id);
        if (ideia != null)
        {
            _context.Ideias.Remove(ideia);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Ideias.CountAsync(i => i.Id == id) > 0;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Ideias.CountAsync();
    }
}
