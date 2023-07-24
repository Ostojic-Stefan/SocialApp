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
    public DbSet<FriendRequests> FriendRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CommentConfig());
        builder.ApplyConfiguration(new PostConfig());
        builder.ApplyConfiguration(new UserProfileConfig());

        //builder.Entity<FriendRequests>()
        //    .HasKey(fr => new { fr.UserProfileFrom, fr.UserProfileTo });

        builder.Entity<FriendRequests>()
            .HasOne(fr => fr.UserProfileFrom)
            .WithMany()
            .HasForeignKey(fr => fr.UserProdileIdFrom);

        builder.Entity<FriendRequests>()
            .HasOne(fr => fr.UserProfileTo)
            .WithMany(u => u.FriendRequests)
            .HasForeignKey(fr => fr.UserProdileIdTo);

        //builder.Entity<FriendRequests>()
        //    .HasOne(f => f.UserProfileFrom)
        //    .WithMany(u => u.FriendRequestsFrom)
        //    .HasForeignKey(f => f.UserProdileIdFrom);

        //builder.Entity<FriendRequests>()
        //    .HasOne(f => f.UserProfileTo)
        //    .WithMany(u => u.FriendRequestsTo)
        //    .HasForeignKey(f => f.UserProdileIdTo);
    }
}
