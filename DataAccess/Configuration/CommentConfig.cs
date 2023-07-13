using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialApp.Domain;

namespace DataAccess.Configuration;

internal class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(comment => comment.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(comment => comment.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(comment => comment.Contents).HasColumnType("VARCHAR(240)").IsRequired();
    }
}
