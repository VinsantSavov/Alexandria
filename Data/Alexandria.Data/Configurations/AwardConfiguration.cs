namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AwardConfiguration : IEntityTypeConfiguration<Award>
    {
        public void Configure(EntityTypeBuilder<Award> award)
        {
            award.Property(a => a.Name)
                 .HasMaxLength(GlobalConstants.AwardNameMaxLength)
                 .IsRequired();
        }
    }
}
