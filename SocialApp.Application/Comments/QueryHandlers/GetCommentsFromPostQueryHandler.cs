using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Query;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Comments.QueryHandlers;

internal class GetCommentsFromPostQueryHandler
    : DataContextRequestHandler<GetCommentsFromPostQuery, Result<IReadOnlyList<CommentResponse>>>
{
    private readonly IMapper _mapper;

    public GetCommentsFromPostQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<CommentResponse>>> Handle(
        GetCommentsFromPostQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<CommentResponse>>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            if (await postRepo.GetByIdAsync(request.PostId, cancellationToken) is null)
            {
                result.AddError(AppErrorCode.NotFound, $"post with id of {request.PostId} does not exist");
                return result;
            }
            var commentRepo = _unitOfWork.CreateReadOnlyRepository<Comment>();
            var comments = await commentRepo
                .Query()
                .Where(c => c.PostId == request.PostId)
                .ToListAsync(cancellationToken);
            result.Data = _mapper.Map<IReadOnlyList<CommentResponse>>(comments);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
