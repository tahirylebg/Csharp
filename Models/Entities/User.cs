namespace TaskBoard.API.Models.Entities;

/* 
 * Modèle de données pour l'entité User
 * Représente un utilisateur de l'application
 * Contient les propriétés nécessaires pour l'authentification et la gestion des utilisateurs
 */

public class User{
    public int Id { get; set; } // Clé primaire 
    public string Username { get; set; } = = string.Empty;
    public string Email { get; set; } = = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; } // Date d'expiration du token de rafraîchissement
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création de l'utilisateur


    // Navigation properties
    public ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>(); // Un utilisateur peut appartenir à plusieurs espaces de travail
    public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Un utilisateur peut faire plusieurs commentaires
}   

