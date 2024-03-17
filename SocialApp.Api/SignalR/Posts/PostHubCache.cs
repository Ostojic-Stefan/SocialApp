using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Domain;
using System.Collections.Concurrent;

namespace SocialApp.Api.SignalR.Posts;

public class PostHubCache
{
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, string>> _userCache = new();
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PostHub> _logger;

    public PostHubCache(IUnitOfWork unitOfWork, ILogger<PostHub> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task AddUser(Guid postId, Guid userId)
    {
        var users = GetUsersOnPost(postId);
        var username = await GetUsername(postId, userId);
        if (!users.TryAdd(userId, username))
        {
            _logger.LogError($"Failed to add {userId} to connections");
        }
    }

    public void RemoveUser(Guid postId, Guid userId)
    {
        var users = GetUsersOnPost(postId);
        if (!users.TryRemove(userId, out var username))
        {
            _logger.LogError($"Failed To remove user with user id: {userId}");
        }
    }

    public async Task<string> GetUsername(Guid postId, Guid userId)
    {
        var users = GetUsersOnPost(postId);
        if (users.TryGetValue(userId, out var name))
        {
            return name;
        }
        var userRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
        name = await userRepo.QueryById(userId).Select(u => u.Username).SingleAsync();
        return name;
    }

    private ConcurrentDictionary<Guid, string> GetUsersOnPost(Guid postId)
    {
        return _userCache.GetOrAdd(postId, k => new ConcurrentDictionary<Guid, string>());
    }
}
