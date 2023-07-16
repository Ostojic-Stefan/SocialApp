using AutoMapper;
using SocialApp.Application.Comments.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.MappingProfiles;

internal class CommentProfiles : Profile
{
	public CommentProfiles()
	{
		CreateMap<Comment, CommentResponse>();
	}
}
