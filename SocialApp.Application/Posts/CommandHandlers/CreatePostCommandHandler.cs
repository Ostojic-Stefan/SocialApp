using EfCoreHelpers;
using SocialApp.Application.Posts.Commands;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class CreatePostCommandHandler
    : DataContextRequestHandler<CreatePostCommand, Post>
{
    public CreatePostCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Post> Handle(CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var post = Post.CreatePost(request.ImageUrl, request.Contents, request.UserProfileId);
        var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
        postRepo.Add(post);
        await _unitOfWork.SaveAsync(cancellationToken);

        return post;
    }
}
