namespace Alexandria.Data.Configurations
{
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StarRatingConfiguration : IEntityTypeConfiguration<StarRating>
    {
        public void Configure(EntityTypeBuilder<StarRating> rating)
        {
            rating.HasOne(r => r.User)
                  .WithMany(u => u.Ratings)
                  .HasForeignKey(r => r.UserId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);

            rating.HasOne(r => r.Book)
                  .WithMany(b => b.Ratings)
                  .HasForeignKey(r => r.BookId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
