namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EditionLanguageConfiguration : IEntityTypeConfiguration<EditionLanguage>
    {
        public void Configure(EntityTypeBuilder<EditionLanguage> language)
        {
            language.Property(l => l.Name)
                    .HasMaxLength(GlobalConstants.EditionLanguageLength)
                    .IsRequired();
        }
    }
}
