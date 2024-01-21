using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialApp.Domain;

namespace DataAccess.Configuration;

internal class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
            .HasMany(comment => comment.Comments)
            .WithOne(comment => comment.Post)
            .HasForeignKey(comment => comment.PostId)
            .HasPrincipalKey(post => post.Id);

        builder.HasOne(post => post.UserProfile);

        builder.Property(post => post.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(post => post.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        //builder.Property(post => post.ImageUrl).HasColumnType("VARCHAR(200)").IsRequired();
        builder.Property(post => post.Contents).HasColumnType("VARCHAR(240)").IsRequired();

        builder
            .HasMany(post => post.Images)
            .WithOne(image => image.Post)
            .HasForeignKey(image => image.PostId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
