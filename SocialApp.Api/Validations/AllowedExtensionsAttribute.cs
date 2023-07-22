using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Validations;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(params string[] extensions)
	{
        _extensions = extensions.Select(ext => ext.ToLower()).ToArray();
    }

    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        if (value is not IFormFile file) 
            return null;
        var extension = Path.GetExtension(file.FileName);
        if (extension is null) 
            return new ValidationResult("File must have an extension");
        return _extensions.Contains(extension.ToLower()) 
            ? ValidationResult.Success 
            : new ValidationResult($"{extension} is not allowed");
    }
}
