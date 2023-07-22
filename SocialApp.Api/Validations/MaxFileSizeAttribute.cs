using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Validations;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        if (value is not IFormFile file) return null;

        return (file.Length > _maxFileSize) switch
        {
            true => new ValidationResult($"Max file size is {_maxFileSize} bytes"),
            false => ValidationResult.Success
        };
    }
}
