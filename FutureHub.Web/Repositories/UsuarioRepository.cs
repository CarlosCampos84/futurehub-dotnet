using Microsoft.EntityFrameworkCore;
using FutureHub.Web.Data;
using FutureHub.Web.Models;
using FutureHub.Web.Repositories.Interfaces;

namespace FutureHub.Web.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly OracleDbContext _context;

    public UsuarioRepository(OracleDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByIdAsync(string id)
    {
        return await _context.Usuarios
            .Include(u => u.AreaInteresse)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .Include(u => u.AreaInteresse)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .Include(u => u.AreaInteresse)
            .ToListAsync();
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task DeleteAsync(string id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Usuarios.CountAsync(u => u.Id == id) > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Usuarios.CountAsync(u => u.Email == email) > 0;
    }
}
