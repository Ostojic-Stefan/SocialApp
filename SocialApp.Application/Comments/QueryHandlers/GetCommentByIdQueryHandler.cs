using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Query;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Comments.QueryHandlers;

internal class GetCommentByIdQueryHandler
    : DataContextRequestHandler<GetCommentByIdQuery, Result<CommentResponse>>
{
    private readonly IMapper _mapper;

    public GetCommentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<CommentResponse>> Handle(GetCommentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<CommentResponse>();
        try
        {
            var commentRepo = _unitOfWork.CreateReadOnlyRepository<Comment>();
            var comment = await commentRepo
                .Query()
                .Where(c => c.Id == request.CommentId)
                .ProjectTo<CommentResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            if (comment is null)
            {
                result.AddError(AppErrorCode.NotFound,
                    $"Comment With the id of {request.CommentId} does not exist");
                return result;
            }
            result.Data = comment;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
