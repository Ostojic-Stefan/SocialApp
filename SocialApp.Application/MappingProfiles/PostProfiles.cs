using AutoMapper;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.MappingProfiles;
internal class PostProfiles : Profile
{
	public PostProfiles()
	{
		CreateMap<Post, PostResponse>();
	}
}
