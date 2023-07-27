using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Identity.Queries;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;
using System.Linq;

namespace SocialApp.Application.Identity.QueryHandlers;

internal class GetUserInformationQueryHandler
    : DataContextRequestHandler<GetUserInformationQuery, Result<GetUserInformationResponse>>
{
    private readonly IMapper _mapper;

    public GetUserInformationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<GetUserInformationResponse>> Handle(GetUserInformationQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<GetUserInformationResponse>();
        try
        {
            var userProfileRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();

            var userFriendRequestsResponse = await userProfileRepo
                .QueryById(request.UserProfileId)
                .SelectMany(u => u.ReceivedFriendRequests)
                .Where(fr => fr.Status == FriendRequestStatus.Pending)
                .ProjectTo<FriendRequestResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var commentsOnUsersPosts = await postRepo
                .Query()
                .Where(p => p.UserProfileId == request.UserProfileId)
                .SelectMany(p => p.Comments)
                .Where(c => !c.SeenByPostOwner)
                .ProjectTo<CommentOnPost>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var likesOnUsersPosts = await postRepo
                .Query()
                .Where(p => p.UserProfileId == request.UserProfileId)
                .SelectMany(p => p.Likes)
                .Where(l => !l.SeenByPostOwner)
                .ProjectTo<LikeOnPost>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);


            var userProfile = await userProfileRepo
                .QueryById(request.UserProfileId)
                .ProjectTo<UserInformation>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var final = new GetUserInformationResponse
            {
                FriendRequests = userFriendRequestsResponse,
                UserInformation = userProfile,
                Notifications = new Notifications
                {
                    CommentsOnPost = commentsOnUsersPosts,
                    LikesOnPost = likesOnUsersPosts
                }
            };

            result.Data = final;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
