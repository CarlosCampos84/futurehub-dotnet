using FutureHub.Web.Models;
using FutureHub.Web.Models.DTOs;
using FutureHub.Web.Repositories.Interfaces;
using FutureHub.Web.Services.Interfaces;

namespace FutureHub.Web.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IAreaRepository areaRepository,
        ILogger<UsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _areaRepository = areaRepository;
        _logger = logger;
    }

    public async Task<UsuarioDTO?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Buscando usuário por ID: {Id}", id);
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        return usuario != null ? UsuarioDTO.FromEntity(usuario) : null;
    }

    public async Task<UsuarioDTO?> GetByEmailAsync(string email)
    {
        _logger.LogInformation("Buscando usuário por email: {Email}", email);
        var usuario = await _usuarioRepository.GetByEmailAsync(email);
        return usuario != null ? UsuarioDTO.FromEntity(usuario) : null;
    }

    public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
    {
        _logger.LogInformation("Listando todos os usuários");
        var usuarios = await _usuarioRepository.GetAllAsync();
        return usuarios.Select(UsuarioDTO.FromEntity);
    }

    public async Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO dto)
    {
        _logger.LogInformation("Criando novo usuário: {Email}", dto.Email);

        // Validar se email já existe
        if (await _usuarioRepository.EmailExistsAsync(dto.Email))
        {
            _logger.LogWarning("Tentativa de criar usuário com email duplicado: {Email}", dto.Email);
            throw new InvalidOperationException("Email já cadastrado");
        }

        // Validar área de interesse se fornecida
        if (!string.IsNullOrEmpty(dto.AreaInteresseId))
        {
            if (!await _areaRepository.ExistsAsync(dto.AreaInteresseId))
            {
                _logger.LogWarning("Área de interesse não encontrada: {AreaId}", dto.AreaInteresseId);
                throw new InvalidOperationException("Área de interesse não encontrada");
            }
        }

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            AreaInteresseId = dto.AreaInteresseId,
            Role = "ROLE_USER"
        };

        var created = await _usuarioRepository.CreateAsync(usuario);
        _logger.LogInformation("Usuário criado com sucesso: {Id}", created.Id);

        return UsuarioDTO.FromEntity(created);
    }

    public async Task<UsuarioDTO> UpdateAsync(string id, UsuarioUpdateDTO dto)
    {
        _logger.LogInformation("Atualizando usuário: {Id}", id);

        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            _logger.LogWarning("Usuário não encontrado para atualização: {Id}", id);
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        if (!string.IsNullOrEmpty(dto.Nome))
            usuario.Nome = dto.Nome;

        if (dto.AreaInteresseId != null)
        {
            if (!string.IsNullOrEmpty(dto.AreaInteresseId) && !await _areaRepository.ExistsAsync(dto.AreaInteresseId))
            {
                _logger.LogWarning("Área de interesse não encontrada: {AreaId}", dto.AreaInteresseId);
                throw new InvalidOperationException("Área de interesse não encontrada");
            }
            usuario.AreaInteresseId = dto.AreaInteresseId;
        }

        var updated = await _usuarioRepository.UpdateAsync(usuario);
        _logger.LogInformation("Usuário atualizado com sucesso: {Id}", id);

        return UsuarioDTO.FromEntity(updated);
    }

    public async Task DeleteAsync(string id)
    {
        _logger.LogInformation("Deletando usuário: {Id}", id);

        if (!await _usuarioRepository.ExistsAsync(id))
        {
            _logger.LogWarning("Usuário não encontrado para deleção: {Id}", id);
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        await _usuarioRepository.DeleteAsync(id);
        _logger.LogInformation("Usuário deletado com sucesso: {Id}", id);
    }
}
