namespace Alexandria.Data.Configurations
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> message)
        {
            message.Property(m => m.Content)
                   .HasMaxLength(GlobalConstants.MessageContentMaxLength)
                   .IsRequired(true);

            message.HasOne(m => m.Author)
                   .WithMany(u => u.SentMessages)
                   .HasForeignKey(m => m.AuthorId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);

            message.HasOne(m => m.Receiver)
                   .WithMany(u => u.ReceivedMessages)
                   .HasForeignKey(m => m.ReceiverId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
