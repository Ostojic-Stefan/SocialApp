using EfCoreHelpers;

namespace SocialApp.Domain;

public enum FriendRequestUpdateStatus
{
    Accept, Reject
}


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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public UserProfile? SenderUser { get; set; }
    public UserProfile? ReceiverUser { get; set; }
}
