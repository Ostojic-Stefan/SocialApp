using AutoMapper;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.MappingProfiles;
internal class PostProfiles : Profile
{
	public PostProfiles()
	{
		CreateMap<UserProfile, UserInfo>();
		CreateMap<Post, PostResponse>()
			.ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile));
	}
}
