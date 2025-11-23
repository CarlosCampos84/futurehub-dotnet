using FutureHub.Web.Models;
using FutureHub.Web.Models.DTOs;

namespace FutureHub.Web.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponse> LoginAsync(LoginDTO dto);
    Task<TokenResponse> RegisterAsync(RegisterDTO dto);
    string GenerateJwtToken(Usuario usuario);
}
