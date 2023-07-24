using EfCoreHelpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Domain;

public enum FriendRequestStatus
{
    Pending,
    Accepted,
    Denied
}

public class FriendRequests : BaseEntity
{
    public Guid UserProdileIdFrom { get; set; }
    public Guid UserProdileIdTo { get; set; }
    public FriendRequestStatus Status { get; set; }

    public UserProfile UserProfileFrom { get; set; }
    public UserProfile UserProfileTo { get; set; }
}
