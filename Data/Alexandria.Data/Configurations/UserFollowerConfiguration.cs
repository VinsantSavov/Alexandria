namespace Alexandria.Data.Configurations
{
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserFollowerConfiguration : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> userFollower)
        {
            userFollower.HasOne(uf => uf.User)
                        .WithMany(u => u.Followers)
                        .HasForeignKey(uf => uf.UserId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict);

            userFollower.HasOne(uf => uf.Follower)
                        .WithMany(u => u.Following)
                        .HasForeignKey(uf => uf.FollowerId)
                        .IsRequired(true)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
