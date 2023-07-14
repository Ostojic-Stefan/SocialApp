using AutoMapper;
using SocialApp.Api.Contracts.Identity;
using SocialApp.Api.Contracts.Posts.Requests;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Posts.Commands;

namespace SocialApp.Api.MappingProfiles;

public class PostMappingProfiles : Profile
{
	public PostMappingProfiles()
	{
		CreateMap<CreatePostRequest, CreatePostCommand>();
		CreateMap<RegisterRequest, RegisterCommand>();
	}
}
