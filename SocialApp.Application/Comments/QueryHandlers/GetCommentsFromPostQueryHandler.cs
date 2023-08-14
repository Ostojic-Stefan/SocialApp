using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Query;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Comments.QueryHandlers;

internal class GetCommentsFromPostQueryHandler
    : DataContextRequestHandler<GetCommentsFromPostQuery, Result<CommentsOnAPostResponse>>
{
    private readonly IMapper _mapper;

    public GetCommentsFromPostQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }
    public override async Task<Result<CommentsOnAPostResponse>> Handle(
        GetCommentsFromPostQuery request, CancellationToken cancellationToken)
    {
        var result = new Result<CommentsOnAPostResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();

            Guid foundPost = await postRepo
                .QueryById(request.PostId)
                .Select(p => p.UserProfileId)
                .FirstOrDefaultAsync(cancellationToken);

            if (foundPost == Guid.Empty)
            {
                result.AddError(AppErrorCode.NotFound, $"post with id of {request.PostId} does not exist");
                return result;
            }
            var commentRepo = _unitOfWork.CreateReadWriteRepository<Comment>();
            var comments = await commentRepo
                .Query()
                .Include(c => c.UserProfile)
                .Where(c => c.PostId == request.PostId)
                .ToListAsync(cancellationToken);

            if (foundPost == request.CurrentUserId)
                comments.ForEach(comment => comment.SetCommentAsSeen());

            result.Data = new CommentsOnAPostResponse
            {
                PostId = request.PostId,
                Comments = _mapper.Map<IReadOnlyList<CommentResponse>>(comments)
            };

            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }

}
