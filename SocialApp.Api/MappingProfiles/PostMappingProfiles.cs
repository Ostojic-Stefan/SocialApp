using AutoMapper;
using SocialApp.Api.Requests.Identity;
using SocialApp.Api.Requests.Posts;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Posts.Commands;

namespace SocialApp.Api.MappingProfiles;

public class PostMappingProfiles : Profile
{
	public PostMappingProfiles()
	{
		CreateMap<CreatePostRequest, CreatePostCommand>();
		CreateMap<RegisterRequest, RegisterCommand>();
		CreateMap<LoginRequest, LoginCommand>();
    }
}
