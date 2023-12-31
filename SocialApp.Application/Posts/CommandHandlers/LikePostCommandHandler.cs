﻿using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class LikePostCommandHandler : DataContextRequestHandler<LikePostCommand, Result<PostLikeAddResponse>>
{
    private readonly IMapper _mapper;

    public LikePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostLikeAddResponse>> Handle(LikePostCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostLikeAddResponse>();
        try
        {
            var repo = _unitOfWork.CreateReadWriteRepository<Post>();
            var post = await repo.Query()
                .Include(p => p.Likes)
                .ThenInclude(l => l.UserProfile)
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);
            if (post is null)
            {
                result.AddError(
                    AppErrorCode.NotFound,
                    $"Post with id of {request.PostId} does not exist");
                return result;
            }
            if (post.Likes.Any(l => l.UserProfile.Id == request.UserProfileId))
            {
                result.AddError(AppErrorCode.DuplicateEntry, $"You Have already liked this post");
                return result;
            }
            var postLike = PostLike.Create(request.PostId,
                request.UserProfileId, request.LikeReaction);
            post.AddLike(postLike);
            await _unitOfWork.SaveAsync(cancellationToken);

            result.Data = new PostLikeAddResponse
            {
                LikeId = postLike.Id,
                PostId = post.Id
            };
            //result.Data = _mapper.Map<PostLikeAddResponse>(post);
            //result.Data = _mapper.Map<PostResponse>(post);
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
