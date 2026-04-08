namespace TaskBoard.API.Models.DTOs.Auth;
/*
 * DTO pour l'inscription d'un utilisateur.
 * Contains est les informations nécessaires pour créer un nouveau compte d'utilisateur.
 */
public class RegisterDto
{
    public string Username { get; set; } = string.Empty; // Nom d'utilisateur choisi par l'utilisateur.
    public string Email { get; set; } = string.Empty; // Adresse e-mail de l'utilisateur, utilisée pour la communication et la récupération de mot de passe.
    public string Password { get; set; } = string.Empty; // Mot de passe choisi par l'utilisateur, utilisé pour sécuriser l'accès à son compte.
}