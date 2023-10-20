using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Query;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Comments.QueryHandlers;

internal class GetCommentsForUserQueryHandler
    : DataContextRequestHandler<GetCommentsForUserQuery, Result<IReadOnlyList<CommentResponse>>>
{
    private readonly IMapper _mapper;

    public GetCommentsForUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<CommentResponse>>> Handle(GetCommentsForUserQuery request, CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<CommentResponse>>();
        try
        {
            var commentRepo = _unitOfWork.CreateReadOnlyRepository<Comment>();
            var comments = await commentRepo
                .Query()
                .Where(c => c.UserProfile.Username.Equals(request.Username))
                .ProjectTo<CommentResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            result.Data = comments;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}