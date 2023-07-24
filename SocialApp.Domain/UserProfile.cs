using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;

namespace SocialApp.Domain;

public class UserProfile : BaseEntity
{
    private readonly ICollection<PostLike> _likes;
    private readonly ICollection<UserProfile> _friends;
    private readonly ICollection<FriendRequests> _friendRequestsTo;
    private readonly ICollection<FriendRequests> _friendRequestsFrom;
    
    private UserProfile() 
    {
        _likes = new List<PostLike>();
        _friends = new List<UserProfile>();
        _friendRequestsTo = new List<FriendRequests>();
        _friendRequestsFrom = new List<FriendRequests>();
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
    //public List<FriendRequests> FriendRequestsTo { get; set; } = new();

    // Friend Requests This Entity Sent
    //public List<FriendRequests> FriendRequestsFrom { get; set; } = new();
    public List<FriendRequests> FriendRequests { get; set; } = new();

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
        FriendRequests.Add(new FriendRequests
        {
            UserProdileIdTo = Id,
            UserProdileIdFrom = userFrom,
            Status = FriendRequestStatus.Pending
        });
    }

    public void AcceptFriendRequest(Guid userId)
    {
        var foundRequest = FriendRequests
            .FirstOrDefault(fr => fr.UserProdileIdFrom == userId);
        if (foundRequest is null)
        {
            // validation exception
            return;
        }
        foundRequest.Status = FriendRequestStatus.Accepted;
        Friends.Add(foundRequest.UserProfileFrom);
    }

    public void UpdateUserProfile(string newAvatarUrl)
    {
        AvatarUrl = newAvatarUrl;
    }
}