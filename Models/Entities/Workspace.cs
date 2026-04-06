namespace TaskBoard.API.Models.Entities;
/* 
 * Modèle de données pour l'entité Workspace
 * Représente un espace de travail dans l'application
 * Contient les propriétés nécessaires pour la gestion des espaces de travail
 */
public class Workspace
{
    public int Id { get; set; } // Clé primaire 
    public string Name { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création de l'espace de travail

    // Clé étrangère
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    // Navigation
    public ICollection<Board> Boards { get; set; } = new List<Board>(); // Un espace de travail peut contenir plusieurs tableaux
}