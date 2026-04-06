namespace TaskBoard.API.Models.Entities;

/* 
 * Modèle de données pour l'entité BoardList
 * Représente une liste dans un tableau
 * Contient les propriétés nécessaires pour la gestion des listes
 */

public class BoardList
{
    public int Id { get; set; } // Clé primaire
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }  // Pour le drag & drop
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création de la liste

    // Clé étrangère
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;

    // Navigation
    public ICollection<Card> Cards { get; set; } = new List<Card>(); // Une liste peut contenir plusieurs cartes
}