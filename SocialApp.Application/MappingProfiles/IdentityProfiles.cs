using AutoMapper;
using SocialApp.Application.Identity.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.MappingProfiles;

internal class IdentityProfiles : Profile
{
	public IdentityProfiles()
	{
		CreateMap<UserProfile, GetUserInformationResponse>();
	}
}
