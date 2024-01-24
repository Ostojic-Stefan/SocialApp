using AutoMapper;
using EfCoreHelpers;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Services.BackgroundServices.ImageProcessing;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;
using System.Threading.Channels;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class CreatePostCommandHandler
    : DataContextRequestHandler<CreatePostCommand, Result<bool>>
{
    private readonly IMapper _mapper;
    private readonly ChannelWriter<ImageProcessingMessage> _channelWriter;

    public CreatePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, Channel<ImageProcessingMessage> channel) 
        : base(unitOfWork)
    {
        _mapper = mapper;
        _channelWriter = channel.Writer;
    }

    public override async Task<Result<bool>> Handle(CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var post = Post.CreatePost(request.Title, request.Contents, request.UserProfileId);
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
            postRepo.Add(post);
            await _unitOfWork.SaveAsync(cancellationToken);
            await _channelWriter.WriteAsync(new ImageProcessingMessage(request.ImageUrl, post.Id, ImageFor.Post), cancellationToken);
            result.Data = true;
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
