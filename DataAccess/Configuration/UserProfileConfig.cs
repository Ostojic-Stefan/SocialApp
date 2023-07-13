using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialApp.Domain;

namespace DataAccess.Configuration;

internal class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        //builder
        //    .HasMany(user => user.Posts)
        //    .WithOne(p => p.User)
        //    .HasForeignKey(post => post.UserId)
        //    .HasPrincipalKey(user => user.Id);

        //builder
        //    .HasMany(user => user.Comments)
        //    .WithOne(comment => comment.User)
        //    .HasForeignKey(comment => comment.UserId)
        //    .HasPrincipalKey(user => user.Id);

        //builder.HasOne(user => user.IdentityUser);

        builder.Property(user => user.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(user => user.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(user => user.Username).HasColumnType("VARCHAR(30)").IsRequired();
        builder.Property(user => user.Biography).HasColumnType("VARCHAR(200)");
        builder.Property(user => user.AvatarUrl).HasColumnType("VARCHAR(200)");
        //builder.Property(user => user.Email).HasColumnType("VARCHAR(40)").IsRequired();
        //builder.Property(user => user.Password).HasColumnType("VARCHAR(50)").IsRequired();
        //builder.Property(user => user.Status).HasConversion<string>();
    }
}
