using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;

namespace SocialApp.Domain;

public class UserProfile : BaseEntity
{
    private UserProfile() 
    {
    }

    //public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Username { get; private set; }
    public string? Biography { get; private set; }
    public string? AvatarUrl { get; private set; }

    // Relationships
    public string IdentityId { get; private set; }
    public IdentityUser IdentityUser { get; private set; }

    public static UserProfile CreateUserProfle(string identityId, string userName, string? biography, string? avararUrl)
    {
        // TODO: Add Validation
        return new UserProfile
        {
            IdentityId = identityId,
            Username = userName,
            Biography = biography,
            AvatarUrl = avararUrl
        };
    }
}