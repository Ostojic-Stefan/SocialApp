using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class CreatePostCommandHandler
    : DataContextRequestHandler<CreatePostCommand, Result<PostResponse>>
{
    private readonly IMapper _mapper;

    public CreatePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostResponse>> Handle(CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostResponse>();
        try
        {
            var post = Post.CreatePost(request.ImageUrl, request.Contents, request.UserProfileId);
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
            postRepo.Add(post);
            await _unitOfWork.SaveAsync(cancellationToken);

            var newPost = await postRepo.Query()
                .ProjectTo<PostResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == post.Id, cancellationToken);

            result.Data = newPost;
        }
        catch (ModelInvalidException ex)
        {
            result.AddError(AppErrorCode.ValidationError, ex.ValidationErrors);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
