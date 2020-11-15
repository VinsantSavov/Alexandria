namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> author)
        {
            author.Property(a => a.FirstName)
                  .HasMaxLength(GlobalConstants.AuthorNamesMaxLength)
                  .IsRequired();

            author.Property(a => a.SecondName)
                  .HasMaxLength(GlobalConstants.AuthorNamesMaxLength)
                  .IsRequired(false);

            author.Property(a => a.LastName)
                  .HasMaxLength(GlobalConstants.AuthorNamesMaxLength)
                  .IsRequired();

            author.Property(a => a.ProfilePicture)
                  .HasMaxLength(GlobalConstants.AuthorProfilePictureLength)
                  .IsRequired(false);

            author.HasOne(a => a.Country)
                  .WithMany(c => c.Authors)
                  .HasForeignKey(a => a.CountryId)
                  .OnDelete(DeleteBehavior.Restrict);

            author.Property(a => a.Biography)
                  .HasMaxLength(GlobalConstants.AuthorBiographyMaxLength)
                  .IsRequired(false);
        }
    }
}
