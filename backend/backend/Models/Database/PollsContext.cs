using backend.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;
using Stripe;

namespace backend.Models.Database;


public class PollsContext : DbContext
{

    private const string DATABASE_PATH = "Polls.db";

    // Tablas



    public DbSet<User> Users { get; set; }

    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollOption> PollOption { get; set; }

    public DbSet<Vote> Votes { get; set; }

    // Crea archivo SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;

        // ruta a servidor despliegue
        string stringConnection = "Server=db21006.databaseasp.net; Database=db21006; Uid=db21006; Pwd=Jn7#-Xm42=Yf;";

        // aqui especifica si usa sqlite o mysql en produccion
#if DEBUG
        optionsBuilder.UseSqlite($"DataSource={basedir}{DATABASE_PATH}");

#else
        optionsBuilder.UseMySql(stringConnection, ServerVersion.AutoDetect(stringConnection));

#endif

    }

    // Configuración de relaciones
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación muchos-a-muchos entre Vote y PollOption
        modelBuilder.Entity<Vote>()
            .HasMany(v => v.SelectedOptions)
            .WithMany()
            .UsingEntity(j => j.ToTable("VoteOptions"));
    }
}