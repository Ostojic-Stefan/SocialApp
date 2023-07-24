using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;

namespace SocialApp.Domain;

public class UserProfile : BaseEntity
{
    private readonly ICollection<PostLike> _likes;
    private UserProfile() 
    {
        _likes = new List<PostLike>();
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
    public IEnumerable<PostLike>? Likes => _likes;

    public List<UserProfile> Friends { get; set; } = new();

    // FriendRequest Relations

    // Friend Requests Sent To This Entity
    public List<FriendRequests> FriendRequestsTo { get; set; } = new();

    // Friend Requests This Entity Sent
    public List<FriendRequests> FriendRequestsFrom { get; set; } = new();

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

    public void SendFriendRequest(Guid userFrom)
    {
        FriendRequestsTo.Add(new FriendRequests
        {
            UserProdileIdFrom = userFrom,
            UserProdileIdTo = Id,
            Status = FriendRequestStatus.Pending
        });
    }

    public void AcceptFriendRequest(Guid userId)
    {
        var foundRequest = FriendRequestsTo
            .FirstOrDefault(x => x.UserProdileIdFrom == userId);
        if (foundRequest is null)
        {
            // validation exception
            return;
        }
        FriendRequestsTo.Remove(foundRequest);
        Friends.Add(foundRequest.UserProfileFrom);
    }

    public void UpdateUserProfile(string newAvatarUrl)
    {
        AvatarUrl = newAvatarUrl;
    }
}