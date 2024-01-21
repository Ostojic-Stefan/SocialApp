namespace SocialApp.Application.Settings;

public class ImageFileStorageSettings
{
    public required string TempImageFileDir { get; set; }
    public required string RootFileImageDir { get; set; }
    public required string ImagesHttpEndpoint { get; set; }
}
