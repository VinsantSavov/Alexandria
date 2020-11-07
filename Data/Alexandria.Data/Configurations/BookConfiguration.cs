namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> book)
        {
            book.Property(b => b.Title)
                .HasMaxLength(GlobalConstants.BookTitleMaxLength)
                .IsRequired();

            book.Property(b => b.Summary)
                .HasMaxLength(GlobalConstants.BookSummaryMaxLength)
                .IsRequired();

            book.Property(b => b.PictureURL)
                .HasMaxLength(GlobalConstants.BookPictureUrlMaxLength)
                .IsRequired(false);

            book.Property(b => b.AmazonLink)
                .HasMaxLength(GlobalConstants.BookAmazonLinkMaxLength)
                .IsRequired(false);

            book.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            book.HasOne(b => b.EditionLanguage)
                .WithMany(el => el.Books)
                .HasForeignKey(b => b.EditionLanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
