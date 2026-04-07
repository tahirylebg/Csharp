namespace TaskBoard.API.Models.Entities;

/* 
 * Modèle de données pour l'entité Comment
 * Représente un commentaire sur une carte
 * Contient les propriétés nécessaires pour la gestion des commentaires
 */

 public class Comment {
    public int Id { get; set;} // Clé primaire
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création du commentaire
    public DateTime? UpdatedAt { get; set; } // Date de dernière modification du commentaire

    //Clé étrangere 
    public int CardId { get; set; } // Clé étrangère vers la carte
    public Card Card { get; set; } = null!;

    public int UserId { get; set; } // Clé étrangère vers l'utilisateur
    public User User { get; set; } = null!;
 }