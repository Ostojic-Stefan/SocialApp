﻿using MediatR;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Queries;

public class GetAllPostsQuery : IRequest<Result<IReadOnlyList<Post>>>
{
}
