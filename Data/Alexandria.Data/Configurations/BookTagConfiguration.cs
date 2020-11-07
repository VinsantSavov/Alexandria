namespace Alexandria.Data.Configurations
{
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BookTagConfiguration : IEntityTypeConfiguration<BookTag>
    {
        public void Configure(EntityTypeBuilder<BookTag> bookTag)
        {
            bookTag.HasKey(bt => new { bt.BookId, bt.TagId });

            bookTag.HasOne(bt => bt.Book)
                   .WithMany(b => b.Tags)
                   .HasForeignKey(bt => bt.BookId)
                   .OnDelete(DeleteBehavior.Restrict);

            bookTag.HasOne(bt => bt.Tag)
                   .WithMany(t => t.Books)
                   .HasForeignKey(bt => bt.TagId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
