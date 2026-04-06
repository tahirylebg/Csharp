namespace TaskBoard.API.Models.Entities;

/* 
 * Modèle de données pour l'entité Board
 * Représente un tableau dans un espace de travail
 * Contient les propriétés nécessaires pour la gestion des tableaux
 */

public class Board
{
    public int Id { get; set; } // Clé primaire
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création du tableau

    // Clé étrangère
    public int WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    // Navigation
    public ICollection<BoardList> Lists { get; set; } = new List<BoardList>(); // Un tableau peut contenir plusieurs listes
}