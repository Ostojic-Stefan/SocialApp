using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialApp.Application.Files.Responses;
using SocialApp.Domain;
using System.Linq.Expressions;

namespace SocialApp.Application.UserProfiles.Extensions;

internal static class ImageProjections
{
    public static Expression<Func<Image, ImageResponse>> Projection
    {
        get
        {
            return image => new ImageResponse
            {
                ImageId = image.Id,
                OriginalImagePath = image.OriginalImagePath,
                FullscreenImagePath = image.FullscreenImagePath,
                ThumbnailImagePath = image.ThumbnailImagePath
            };
        }
    }

    public static ImageResponse FromEntity(Image entity)
    {
        return Projection.Compile().Invoke(entity);
    }

    public static Expression<Func<Image, ImageResponse>> ToImageResponse()
    {
        return image => new ImageResponse
        {
            ImageId = image.Id,
            OriginalImagePath = image.OriginalImagePath,
            FullscreenImagePath = image.FullscreenImagePath,
            ThumbnailImagePath = image.ThumbnailImagePath
        };
    }
}
