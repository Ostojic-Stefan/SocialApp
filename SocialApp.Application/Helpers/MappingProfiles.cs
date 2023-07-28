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
        CreateMap<UserProfile, UserInfo>()
            .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.Id));
        CreateMap<Post, PostResponse>()
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile))
            .ForMember(dest => dest.NumLikes, opt => opt.MapFrom(src => src.Likes.Count()));

        CreateMap<Post, PostsForUserResponse>();
        CreateMap<UserProfile, UserInformationResponse>()
            .ForMember(uip => uip.UserProfileId, opt => opt.MapFrom(src => src.Id));

        CreateMap<UserProfile, LikeUserInfo>();

        //CreateMap<PostLike, PostLikeResponse>();
        CreateMap<PostLike, GetLikesForAPostResponse>()
            .ForMember(x => x.UserInformation, opt => opt.MapFrom(src => src.UserProfile));

        CreateMap<UserProfile, UserInformation>()
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<FriendRequest, FriendRequestResponse>()
            .ForMember(x => x.RequesterId, opt => opt.MapFrom(src => src.SenderUserId))
            .ForMember(x => x.RequesterUsername, opt => opt.MapFrom(src => src.SenderUser.Username))
            .ForMember(x => x.RequesterAvatarUrl, opt => opt.MapFrom(src => src.SenderUser.AvatarUrl))
            .ForMember(x => x.RequestTimeSent, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<Comment, CommentOnPost>()
            .ForMember(x => x.CommenterUsername, opt => opt.MapFrom(src => src.UserProfile.Username))
            .ForMember(x => x.CommenterAvatarUrl, opt => opt.MapFrom(src => src.UserProfile.AvatarUrl))
            .ForMember(x => x.CommentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.ContentsReduced, opt => opt.MapFrom(src => src.Contents));

        CreateMap<PostLike, LikeOnPost>()
            .ForMember(x => x.LikerUsername, opt => opt.MapFrom(src => src.UserProfile.Username))
            .ForMember(x => x.LikerAvatarUrl, opt => opt.MapFrom(src => src.UserProfile.AvatarUrl))
            .ForMember(x => x.LikeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.LikeReaction, opt => opt.MapFrom(src => src.LikeReaction));
    }

}
