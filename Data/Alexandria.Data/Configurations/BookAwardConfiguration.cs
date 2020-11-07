namespace Alexandria.Data.Configurations
{
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BookAwardConfiguration : IEntityTypeConfiguration<BookAward>
    {
        public void Configure(EntityTypeBuilder<BookAward> bookAward)
        {
            bookAward.HasKey(ba => new { ba.BookId, ba.AwardId });

            bookAward.HasOne(ba => ba.Book)
                     .WithMany(b => b.Awards)
                     .HasForeignKey(ba => ba.BookId)
                     .OnDelete(DeleteBehavior.Restrict);

            bookAward.HasOne(ba => ba.Award)
                     .WithMany(a => a.Books)
                     .HasForeignKey(ba => ba.AwardId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
