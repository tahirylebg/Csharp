namespace TaskBoard.API.Models.DTOs.Auth;

public class LoginDto
{
    public string Email { get; set; } = string.Empty; // Adresse e-mail de l'utilisateur, utilisée pour l'identification lors de la connexion.
    public string Password { get; set; } = string.Empty; // Mot de passe de l'utilisateur, utilisé pour sécuriser l'accès à son compte lors de la connexion.
}