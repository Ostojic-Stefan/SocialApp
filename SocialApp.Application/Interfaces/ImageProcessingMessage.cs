namespace SocialApp.Application.Interfaces;

public enum ImageFor
{
    User, Post
}

public record ImageProcessingMessage(string FilePath, Guid ResourceId, ImageFor ImageFor);