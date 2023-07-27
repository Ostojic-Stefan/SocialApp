using AutoMapper;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Helpers;

internal class MappingProfiles : Profile
{
	public MappingProfiles()
	{
		CreateMap<Comment, CommentResponse>();
        CreateMap<UserProfile, UserInfo>();
        CreateMap<Post, PostResponse>()
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile));
        CreateMap<Post, PostsForUserResponse>();
        CreateMap<UserProfile, UserInformationResponse>()
            .ForMember(uip => uip.UserProfileId, opt => opt.MapFrom(src => src.Id));
        CreateMap<UserProfile, LikeUserInfo>();
        CreateMap<PostLike, PostLikeResponse>();
        CreateMap<PostLike, GetLikesForAPostResponse>()
            .ForMember(x => x.UserInformation, opt => opt.MapFrom(src => src.UserProfile));

        // GetUserInformation

        CreateMap<UserProfile, UserInformation>()
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.Id));
    }

}
