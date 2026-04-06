namespace TaskBoard.API.Models.Entities;

public class Card
{
    public int Id { get; set; } // Clé primaire
    public string Title { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public int Position { get; set; }  // Pour le drag & drop
    public DateTime? DueDate { get; set; } // Date d'échéance de la carte
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date de création de la carte

    // Clé étrangère
    public int BoardListId { get; set; }
    public BoardList BoardList { get; set; } = null!;

    // Navigation
    public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Une carte peut avoir plusieurs commentaires
    public ICollection<Label> Labels { get; set; } = new List<Label>(); // Une carte peut avoir plusieurs étiquettes
    public ICollection<User> AssignedUsers { get; set; } = new List<User>(); // Une carte peut être assignée à plusieurs utilisateurs
}