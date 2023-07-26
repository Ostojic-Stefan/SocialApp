using EfCoreHelpers;

namespace SocialApp.Domain;

public enum FriendRequestStatus
{
    Pending,
    Accepted,
    Denied
}

public class FriendRequest
{
    public Guid SenderUserId { get; set; }
    public Guid ReceiverUserId { get; set; }
    public FriendRequestStatus Status { get; set; }

    public UserProfile? SenderUser { get; set; }
    public UserProfile? ReceiverUser { get; set; }
}
