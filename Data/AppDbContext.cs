using Microsoft.EntityFrameworkCore; // Import de Entity Framework Core pour la gestion de la base de données
using TaskBoard.API.Models.Entities; // Import des modèles d'entités

namespace TaskBoard.API.Data;

/*
*C'est le pont entre ton code C# et ta base de données PostgreSQL. 
*C'est lui qui dit à EF Core quelles tables créer et comment les relier.
*Il contient des DbSet pour chaque entité, ce qui permet à EF Core de savoir quelles tables créer dans la base de données.
*Il est aussi responsable de la configuration de la connexion à la base de données et de la configuration des relations entre les entités.
*/

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 

    // Une propriété = une table dans PostgreSQL
    public DbSet<User> Users => Set<User>();// Table des utilisateurs
    public DbSet<Workspace> Workspaces => Set<Workspace>(); // Table des espaces de travail
    public DbSet<Board> Boards => Set<Board>(); // Table des tableaux
    public DbSet<BoardList> BoardLists => Set<BoardList>(); // Table des listes
    public DbSet<Card> Cards => Set<Card>(); // Table des cartes
    public DbSet<Comment> Comments => Set<Comment>(); // Table des commentaires
    public DbSet<Label> Labels => Set<Label>(); // Table des étiquettes

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base .OnModelCreating(modelBuilder);

        // User : configuration de l'entité User pour la base de données
        modelBuilder.Entity<User>(entity => {
            entity.HasKey(u => u.Id); // Clé primaire
            entity.HasIndex(u => u.Email).IsUnique(); // Email unique
            entity.HasIndex(u => u.Username).IsUnique(); // Username unique
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100); // Username requis, max 100 caractères
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255); // Email requis, max 255 caractères
            entity.Property(u => u.PasswordHash).IsRequired(); // PasswordHash requis
        });

        // Workspace : configuration de l'entité Workspace pour la base de données
        modelBuilder.Entity<Workspace>(entity => {
            entity.HasKey(w => w.Id); // Clé primaire
            entity.Property(w => w.Name).IsRequired().HasMaxLength(200); // Name requis, max 200 caractères

            entity.HasOne(w => w.Owner) // Relation entre Workspace et User (propriétaire)
                .WithMany(u => u.Workspaces) // Un utilisateur peut avoir plusieurs espaces de travail
                .HasForeignKey(w => w.OwnerId) // Clé étrangère dans Workspace
                .OnDelete(DeleteBehavior.Cascade); // Suppression en cascade
        });


        // Board : configuration de l'entité Board pour la base de données
        modelBuilder.Entity<Board>(entity => {
            entity.HasKey(b => b.Id); // Clé primaire
            entity.Property(b => b.Name).IsRequired().HasMaxLength(200); // Name requis, max 200 caractères

            // Un board appartient à un workspace, un workspace peut avoir plusieurs boards
            entity.HasOne(b => b.Workspace) // Relation entre Board et Workspace
                .WithMany(w => w.Boards) // Un espace de travail peut avoir plusieurs tableaux
                .HasForeignKey(b => b.WorkspaceId) // Clé étrangère dans Board
                .OnDelete(DeleteBehavior.Cascade); // Suppression en cascade
        });

        // BoardList : configuration de l'entité BoardList pour la base de données
        modelBuilder.Entity<BoardList>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Name).IsRequired().HasMaxLength(200); 

            // Une liste appartient à un board, un board peut avoir plusieurs listes
            entity.HasOne(l => l.Board) // Relation entre BoardList et Board
                .WithMany(b => b.Lists) // Un tableau peut avoir plusieurs listes
                .HasForeignKey(l => l.BoardId) // Clé étrangère dans BoardList
                .OnDelete(DeleteBehavior.Cascade); // Suppression en cascade
        });

        // Card : configuration de l'entité Card pour la base de données
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(c => c.Id); // Clé primaire
            entity.Property(c => c.Title).IsRequired().HasMaxLength(500);

            // Une card appartient à une BoardList
            entity.HasOne(c => c.BoardList)
                  .WithMany(l => l.Cards)
                  .HasForeignKey(c => c.BoardListId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relation many-to-many Card <-> User (membres assignés)
            entity.HasMany(c => c.AssignedUsers) // Une carte peut être assignée à plusieurs utilisateurs
                  .WithMany()
                  .UsingEntity(j => j.ToTable("CardAssignees"));

            // Relation many-to-many Card <-> Label
            entity.HasMany(c => c.Labels)
                  .WithMany(l => l.Cards)
                  .UsingEntity(j => j.ToTable("CardLabels"));
        });

        // Comment : configuration de l'entité Comment pour la base de données
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Content).IsRequired().HasMaxLength(1000);

            // Un commentaire appartient à une card
            entity.HasOne(c => c.Card)
                  .WithMany(card => card.Comments)
                  .HasForeignKey(c => c.CardId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Un commentaire appartient à un User
            entity.HasOne(c => c.User)
                  .WithMany(u => u.Comments)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // On garde le commentaire si user supprimé
        });

        // Label : configuration de l'entité Label pour la base de données
        modelBuilder.Entity<Label>(entity => {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Name).IsRequired().HasMaxLength(100);
            entity.Property(l => l.Color).IsRequired().HasMaxLength(7); // ex: #FF0000

            // Un label appartient à un Board
            entity.HasOne(l => l.Board)
                  .WithMany()
                  .HasForeignKey(l => l.BoardId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
