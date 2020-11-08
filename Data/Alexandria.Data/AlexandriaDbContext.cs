namespace Alexandria.Data
{
    using Alexandria.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AlexandriaDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AlexandriaDbContext()
        {
        }

        public AlexandriaDbContext(DbContextOptions<AlexandriaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Award> Awards { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<EditionLanguage> EditionLanguages { get; set; }

        public DbSet<BookAward> BookAwards { get; set; }

        public DbSet<BookGenre> BookGenres { get; set; }

        public DbSet<BookTag> BookTags { get; set; }

        public DbSet<StarRating> StarRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Alexandria;Integrated Security=True");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
