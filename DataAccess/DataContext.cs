using DataAccess.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialApp.Domain;

namespace DataAccess;

public  class DataContext : IdentityDbContext
{
	public DataContext(DbContextOptions<DataContext> options)
		: base(options) {}

	public DbSet<Post> Posts { get; set; }
	public DbSet<UserProfile> UserProfiles { get; set; }
	public DbSet<Comment> Comments { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CommentConfig());
        builder.ApplyConfiguration(new PostConfig());
        builder.ApplyConfiguration(new UserProfileConfig());
    }
}
