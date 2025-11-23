using Microsoft.EntityFrameworkCore;
using FutureHub.Web.Data;
using FutureHub.Web.Models;
using FutureHub.Web.Repositories.Interfaces;

namespace FutureHub.Web.Repositories;

public class AreaRepository : IAreaRepository
{
    private readonly OracleDbContext _context;

    public AreaRepository(OracleDbContext context)
    {
        _context = context;
    }

    public async Task<Area?> GetByIdAsync(string id)
    {
        return await _context.Areas.FindAsync(id);
    }

    public async Task<Area?> GetByNomeAsync(string nome)
    {
        return await _context.Areas
            .FirstOrDefaultAsync(a => a.Nome == nome);
    }

    public async Task<IEnumerable<Area>> GetAllAsync()
    {
        return await _context.Areas.ToListAsync();
    }

    public async Task<Area> CreateAsync(Area area)
    {
        _context.Areas.Add(area);
        await _context.SaveChangesAsync();
        return area;
    }

    public async Task<Area> UpdateAsync(Area area)
    {
        _context.Areas.Update(area);
        await _context.SaveChangesAsync();
        return area;
    }

    public async Task DeleteAsync(string id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area != null)
        {
            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Areas.CountAsync(a => a.Id == id) > 0;
    }
}
