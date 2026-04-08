using TaskBoard.API.Models.DTOs.Auth;

namespace TaskBoard.API.Services.Interfaces;

/*
 * Interface pour le service d'authentification.
 * Contains est les méthodes définissant les opérations d'authentification, telles que l'inscription, la connexion et le rafraîchissement des tokens.
 */

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto); // Méthode pour enregistrer un nouvel utilisateur, prenant un DTO d'inscription et retournant une réponse d'authentification contenant les tokens et les informations de l'utilisateur.
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken); 
}