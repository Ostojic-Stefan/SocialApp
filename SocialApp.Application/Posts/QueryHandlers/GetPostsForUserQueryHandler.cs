using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Extensions;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetPostsForUserQueryHandler :
    DataContextRequestHandler<GetPostsForUserQuery, Result<PostsForUserResponse>>
{
    private readonly IMapper _mapper;

    public GetPostsForUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostsForUserResponse>> Handle(GetPostsForUserQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostsForUserResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var userRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();

            var posts = await postRepo
                .Query()
                .Where(p => p.UserProfile.Id == request.UserProfileId)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.MapToPostReponse(request.UserProfileId))
                .ToListAsync(cancellationToken);

            if (posts is null)
            {
                result.AddError(AppErrorCode.NotFound, $"User with id of {request.UserProfileId} was not found");
                return result;
            }

            var userInfo = await userRepo
                .Query()
                .Where(user => user.Id == request.UserProfileId)
                .ProjectTo<UserInfo>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

            result.Data = new PostsForUserResponse
            {
                Posts = posts,
                UserInfo = userInfo
            };
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
