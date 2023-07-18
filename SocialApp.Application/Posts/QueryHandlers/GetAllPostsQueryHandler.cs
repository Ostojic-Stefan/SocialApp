﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.Services;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler
    : DataContextRequestHandler<GetAllPostsQuery, Result<PagedList<PostResponse>>>
{
    private readonly IMapper _mapper;

    public GetAllPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PagedList<PostResponse>>> Handle(GetAllPostsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PagedList<PostResponse>>();
        try
        {
            var repo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var postsQuery = repo
                .Query()
                .Include(p => p.UserProfile)
                .Select(p => new PostResponse
                { 
                    Contents = p.Contents,
                    CreatedAt = p.CreatedAt,
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    UserInfo = new UserInfo
                    {
                        AvatarUrl = p.UserProfile.AvatarUrl,
                        Biography = p.UserProfile.Biography,
                        Username = p.UserProfile.Username
                    },
                    UpdatedAt = p.UpdatedAt,
                    UserProfileId = p.UserProfileId
                });
                //.ProjectTo<PostResponse>(_mapper.ConfigurationProvider);
            var postPager = new Pager<PostResponse>(request.PageSize, request.PageNumber);
            var paginatedList = await postPager.ToPagedList(postsQuery, cancellationToken);
            result.Data = paginatedList;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
