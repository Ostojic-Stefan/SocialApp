using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetPostsForUserQueryHandler :
    DataContextRequestHandler<GetPostsForUserQuery, Result<IReadOnlyList<PostResponse>>>
{
    private readonly IMapper _mapper;

    public GetPostsForUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<PostResponse>>> Handle(GetPostsForUserQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<PostResponse>>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var posts = await postRepo
                .Query()
                .Where(p => p.UserProfile.Username == request.Username)
                .ProjectTo<PostResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            if (posts is null)
            {
                result.AddError(AppErrorCode.NotFound, $"User with username of {request.Username} was not found");
                return result;
            }
            result.Data = posts;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
