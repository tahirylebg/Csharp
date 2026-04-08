namespace TaskBoard.API.Models.DTOs.Auth;
/*
 * DTO pour la réponse d'authentification.
 * Contains est les informations retournées après une authentification réussie, incluant les tokens d'accès et de rafraîchissement, ainsi que les informations de l'utilisateur.
 */
public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty; // Token d'accès JWT, utilisé pour authentifier les requêtes de l'utilisateur après une connexion réussie.
    public string RefreshToken { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty; // Nom d'utilisateur de l'utilisateur authentifié, utilisé pour l'affichage et la personnalisation de l'expérience utilisateur.
    public string Email { get; set; } = string.Empty;
}