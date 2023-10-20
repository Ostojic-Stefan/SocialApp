using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class GetPostDetailsQueryHandler
    : DataContextRequestHandler<GetPostDetailsQuery, Result<PostDetailsResponse>>
{
    private readonly IMapper _mapper;

    public GetPostDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostDetailsResponse>> Handle(GetPostDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostDetailsResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var post = await postRepo
                .QueryById(request.PostId)
                .ProjectTo<PostDetailsResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (post is null)
            {
                result.AddError(AppErrorCode.NotFound, $"Post with id of {request.PostId} does not exist.");
                return result;
            }

            result.Data = post;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}