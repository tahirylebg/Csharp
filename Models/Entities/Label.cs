namespace TaskBoard.API.Models.Entities;    

public class Label
{
    public int Id { get; set; } // Clé primaire
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000"; // Couleur par défaut (noir)

    // Clé etrangère
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;
}