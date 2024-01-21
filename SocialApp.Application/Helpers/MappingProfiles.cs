using AutoMapper;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.QueryHandlers;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Helpers;

internal class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Comment, CommentResponse>()
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile));

        CreateMap<UserProfile, UserInfo>()
            .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Post, PostResponse>()
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile))
            .ForMember(dest => dest.NumLikes, opt => opt.MapFrom(src => src.Likes.Count()))
            .ForMember(dest => dest.NumComments, opt => opt.MapFrom(src => src.Comments.Count()));

        CreateMap<Post, PostDetailsResponse>()
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile))
            .ForMember(dest => dest.NumLikes, opt => opt.MapFrom(src => src.Likes.Count()))
            .ForMember(dest => dest.NumComments, opt => opt.MapFrom(src => src.Comments.Count()))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserProfile));

        CreateMap<PostLike, PostLikeDeleteResponse>();

        CreateMap<Post, GetLikesForAPostResponse>()
            .ForMember(x => x.LikeInfo, opt => opt.MapFrom(src => src.Likes))
            .ForMember(x => x.PostId, opt => opt.MapFrom(src => src.Id));

        CreateMap<FriendRequest, FriendRequestResponse>()
            .ForMember(x => x.RequesterId, opt => opt.MapFrom(src => src.SenderUserId))
            .ForMember(x => x.RequesterUsername, opt => opt.MapFrom(src => src.SenderUser.Username))
            //.ForMember(x => x.RequesterAvatarUrl, opt => opt.MapFrom(src => src.SenderUser.AvatarUrl))
            .ForMember(x => x.RequestTimeSent, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<UserProfile, FriendResponse>()
            .ForMember(x => x.UserProfileId, opt => opt.MapFrom(src => src.Id));

        CreateMap<PostLike, PostLikeResponse>()
            .ForMember(x => x.UserInformation, opt => opt.MapFrom(src => src.UserProfile));

        CreateMap<PostLike, LikesForUserResponse>()
            .ForMember(x => x.UserInfo, opt => opt.MapFrom(src => src.UserProfile));
    }

}
