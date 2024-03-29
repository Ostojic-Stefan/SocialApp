﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Extensions;
using SocialApp.Application.UserProfiles.Queries;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.QueryHandlers;

internal class GetAllFriendsQueryHandler
    : DataContextRequestHandler<GetAllFriendsQuery, Result<IReadOnlyList<UserInfo>>>
{
    private readonly IMapper _mapper;

    public GetAllFriendsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<UserInfo>>> Handle(GetAllFriendsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<UserInfo>>();
        try
        {
            var repo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var friends = await repo
                .Query()
                .Where(u => u.Id == request.UserId)
                .SelectMany(u => u.Friends)
                .Include(u => u.ProfileImage)
                .Select(u => u.MapToUserInfo())
                .ToListAsync(cancellationToken);

            result.Data = friends;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
