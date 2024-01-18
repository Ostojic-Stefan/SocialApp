using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;
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
                .Select(p => new PostResponse
                {
                    Id = p.Id,
                    Contents = p.Contents,
                    UserInfo = new UserInfo
                    {
                        UserProfileId = p.UserProfileId,
                        Username = p.UserProfile.Username,
                        AvatarUrl = p.UserProfile.AvatarUrl,
                    },
                    ImageUrl = p.ImageUrl,
                    NumComments = p.Comments.Count(),
                    NumLikes = p.Likes.Count(),
                    LikeInfo = p.Likes.Any(l => l.UserProfileId == request.UserProfileId)
                        ? new PostLikeInfo
                        {
                            LikedByCurrentUser = true,
                            LikeId = p.Likes.First(l => l.UserProfileId == request.UserProfileId).Id,
                        }
                        : new PostLikeInfo
                        {
                            LikedByCurrentUser = false,
                            LikeId = Guid.Empty,
                        },
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                })
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
