namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> genre)
        {
            genre.Property(g => g.Name)
                 .HasMaxLength(GlobalConstants.GenreNameMaxLength)
                 .IsRequired();

            genre.Property(g => g.Description)
                 .HasMaxLength(GlobalConstants.GenreDescriptionMaxLength)
                 .IsRequired();
        }
    }
}
