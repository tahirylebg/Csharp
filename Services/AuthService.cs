using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskBoard.API.Data;
using TaskBoard.API.Models.DTOs.Auth;
using TaskBoard.API.Models.Entities;
using TaskBoard.API.Services.Interfaces;

namespace TaskBoard.API.Services;

public class AuthService : IAuthService {

    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context; // Injection du contexte de la base de données pour accéder aux données des utilisateurs et des rôles.
        _configuration = configuration; // Injection de la configuration pour accéder aux paramètres de l'application, tels que les clés secrètes pour la génération de tokens.
    } 
    
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto) {
        // Vérifie si un utilisateur avec l'adresse e-mail fournie existe déjà dans la base de données.
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Nom d'utilisateur déjà pris."); // Si un utilisateur existe déjà, une exception est levée pour indiquer que le nom d'utilisateur est déjà pris.

        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            throw new Exception("Nom d'utilisateur déjà utilisé"); // Si un utilisateur existe déjà, une exception est levée pour indiquer que le nom d'utilisateur est déjà utilisé.

        // Crée un nouvel objet User à partir des informations fournies dans le DTO d'inscription.
        var user = new User
        {
            Username = dto.Username, // Le nom d'utilisateur est défini à partir du DTO d'inscription.
            Email = dto.Email, // L'adresse e-mail est définie à partir du DTO d'inscription.
            PasswordHash = HashPassword(dto.Password) // Le mot de passe est haché et stocké dans la base de données pour des raisons de sécurité.
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return await GenerateTokensAsync(user); // Après l'enregistrement de l'utilisateur, des tokens d'authentification sont générés et retournés dans la réponse.

    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto) {
        // Recherche un utilisateur dans la base de données en fonction de l'adresse e-mail fournie dans le DTO de connexion.
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            throw new Exception("Email ou mot de passe incorrect."); // Si l'utilisateur n'existe pas ou si le mot de passe est incorrect, une exception est levée pour indiquer que les informations d'identification sont invalides.

        return await GenerateTokensAsync(user); // Si les informations d'identification sont valides, des tokens d'authentification sont générés et retournés dans la réponse.
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken) {
        // Recherche un utilisateur dans la base de données en fonction du token de rafraîchissement fourni.
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.RefreshToken == refreshToken &&
            u.RefreshTokenExpiry > DateTime.UtcNow);

        if (user == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
            throw new Exception("Token de rafraîchissement invalide ou expiré."); // Si le token de rafraîchissement est invalide ou expiré, une exception est levée pour indiquer que le token n'est pas valide.

        return await GenerateTokensAsync(user); // Si le token de rafraîchissement est valide, de nouveaux tokens d'authentification sont générés et retournés dans la réponse.
    }

    private async Task<AuthResponseDto> GenerateTokensAsync(User user){
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(); // Génère un nouveau token de rafraîchissement pour l'utilisateur.

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("JwtSettings:RefreshTokenExpirationDays")); // Définit la date d'expiration du token de rafraîchissement en fonction des paramètres de configuration.

        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken, // Le token d'accès JWT généré pour l'utilisateur.
            RefreshToken = refreshToken, // Le token de rafraîchissement généré pour l'utilisateur.
            Username = user.Username, // Le nom d'utilisateur de l'utilisateur authentifié.
            Email = user.Email // L'adresse e-mail de l'utilisateur authentifié.
        };        
    }

}
