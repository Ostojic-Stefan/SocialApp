using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler
    : DataContextRequestHandler<GetAllPostsQuery, Result<IReadOnlyList<PostResponse>>>
{
    private readonly IMapper _mapper;

    public GetAllPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<PostResponse>>> Handle(GetAllPostsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<PostResponse>>();
        var repo = _unitOfWork.CreateReadOnlyRepository<Post>();
        var posts = await repo
            .Query()
            .ProjectTo<PostResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        result.Data = posts;
        return result;
    }
}
