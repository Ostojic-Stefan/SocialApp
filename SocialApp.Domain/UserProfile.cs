using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;

namespace SocialApp.Domain;

public class UserProfile : BaseEntity
{
    private readonly ICollection<PostLike> _likes;
    private readonly ICollection<UserProfile> _friends;
    private readonly ICollection<FriendRequest> _sentFriendRequests;
    private readonly ICollection<FriendRequest> _receivedFriendRequests;
    
    private UserProfile() 
    {
        _likes = new List<PostLike>();
        _friends = new List<UserProfile>();
        _sentFriendRequests = new List<FriendRequest>();
        _receivedFriendRequests = new List<FriendRequest>();
    }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Username { get; private set; }
    public string? Biography { get; private set; }

    public Guid? ProfileImageId { get; set; }
    public Image ProfileImage { get; set; }

    // Relationships
    // TODO: make private set
    public IEnumerable<Image> Images { get; set; } = new List<Image>();
    public string IdentityId { get; private set; }
    public IdentityUser IdentityUser { get; private set; }
    public IEnumerable<PostLike>? Likes => _likes;

    public IEnumerable<UserProfile>? Friends => _friends;
    public IEnumerable<FriendRequest> SentFriendRequests => _sentFriendRequests;
    public IEnumerable<FriendRequest> ReceivedFriendRequests => _receivedFriendRequests;

    public static UserProfile CreateUserProfle(string identityId, string userName, string? biography)
    {
        // TODO: Add Validation
        return new UserProfile
        {
            IdentityId = identityId,
            Username = userName,
            Biography = biography,
            ProfileImage = Image.GenerateDefaultAvatar()
        };
    }

    public void SetProfileImage(Guid imageId)
    {
        ProfileImageId = imageId;
    }

    public void SendFriendRequest(Guid userFrom)
    {
        _receivedFriendRequests.Add(new FriendRequest
        {
            ReceiverUserId = Id,
            SenderUserId = userFrom,
            Status = FriendRequestStatus.Pending
        });
    }

    public void AcceptFriendRequest(Guid userId)
    {
        var foundRequest = ReceivedFriendRequests
            .FirstOrDefault(fr => fr.SenderUserId == userId);
        if (foundRequest is null)
        {
            // TODO: validation exception
            return;
        }
        foundRequest.SenderUser._friends.Add(this);
        foundRequest.Status = FriendRequestStatus.Accepted;
        _friends.Add(foundRequest.SenderUser);
    }

    public void RejectFriendRequest(Guid userId)
    {
        var foundRequest = ReceivedFriendRequests
            .FirstOrDefault(fr => fr.SenderUserId == userId);
        if (foundRequest is null)
        {
            // TODO: validation exception
            return;
        }
        foundRequest.Status = FriendRequestStatus.Denied;
    }

    public void UpdateUserProfile(string newAvatarUrl)
    {
        //AvatarUrl = newAvatarUrl;
        throw new NotImplementedException();
    }
}