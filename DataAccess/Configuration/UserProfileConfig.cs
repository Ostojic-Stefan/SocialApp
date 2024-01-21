using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialApp.Domain;
using System.Reflection.Emit;

namespace DataAccess.Configuration;

internal class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        //builder.HasOne(user => user.IdentityUser);

        builder.HasMany(user => user.Friends)
            .WithMany()
            .UsingEntity(join =>
            {
                join.ToTable("Friends");
                join.Property("FriendsId").HasColumnName("User1");
                join.Property("UserProfileId").HasColumnName("User2");
            });

        builder.Property(user => user.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(user => user.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(user => user.Username).HasColumnType("VARCHAR(30)").IsRequired();
        builder.Property(user => user.Biography).HasColumnType("VARCHAR(200)");
        //builder.Property(user => user.AvatarUrl).HasColumnType("VARCHAR(200)");

        builder
            .HasMany(user => user.Images)
            .WithOne(image => image.User)
            .HasForeignKey(image => image.UserProfileId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(u => u.ProfileImage)
            .WithMany()
            .HasForeignKey(u => u.ProfileImageId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deleting the image if it's a profile image
    }
}
