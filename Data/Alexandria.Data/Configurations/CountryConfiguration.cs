namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> country)
        {
            country.Property(c => c.Name)
                   .HasMaxLength(GlobalConstants.CountryNameMaxLength)
                   .IsRequired();
        }
    }
}
