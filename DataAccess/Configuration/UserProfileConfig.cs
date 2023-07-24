using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialApp.Domain;

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
        builder.Property(user => user.AvatarUrl).HasColumnType("VARCHAR(200)");
    }
}
