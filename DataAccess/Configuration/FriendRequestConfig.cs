using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialApp.Domain;

namespace SocialApp.DataAccess.Configuration;

internal class FriendRequestConfig : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.HasKey(f => new { f.SenderUserId, f.ReceiverUserId });

        builder.HasOne(fr => fr.SenderUser)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(fr => fr.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fr => fr.ReceiverUser)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(fr => fr.ReceiverUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
