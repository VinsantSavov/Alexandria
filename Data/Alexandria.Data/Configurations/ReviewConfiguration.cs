namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> review)
        {
            review.Property(r => r.Description)
                  .HasMaxLength(GlobalConstants.ReviewDescriptionMaxLength)
                  .IsRequired();

            review.HasOne(r => r.Book)
                  .WithMany(b => b.Reviews)
                  .HasForeignKey(r => r.BookId)
                  .OnDelete(DeleteBehavior.Restrict);

            review.HasOne(r => r.Author)
                  .WithMany(a => a.Reviews)
                  .HasForeignKey(r => r.AuthorId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
