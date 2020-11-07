namespace Alexandria.Data.Configurations
{
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> bookGenre)
        {
            bookGenre.HasKey(bg => new { bg.BookId, bg.GenreId });

            bookGenre.HasOne(bg => bg.Book)
                     .WithMany(b => b.Genres)
                     .HasForeignKey(bg => bg.BookId)
                     .OnDelete(DeleteBehavior.Restrict);

            bookGenre.HasOne(bg => bg.Genre)
                     .WithMany(g => g.Books)
                     .HasForeignKey(bg => bg.GenreId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
