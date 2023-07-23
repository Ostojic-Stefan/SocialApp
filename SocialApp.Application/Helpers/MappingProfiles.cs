using AutoMapper;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Helpers;

internal class MappingProfiles : Profile
{
	public MappingProfiles()
	{
		CreateMap<Comment, CommentResponse>();
		CreateMap<UserProfile, GetUserInformationResponse>();
        CreateMap<UserProfile, UserInfo>();
        CreateMap<Post, PostResponse>()
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile));
        CreateMap<Post, PostsForUserResponse>();
        CreateMap<UserProfile, UserInformationResponse>()
            .ForMember(uip => uip.UserProfileId, opt => opt.MapFrom(src => src.Id));
    }

}
