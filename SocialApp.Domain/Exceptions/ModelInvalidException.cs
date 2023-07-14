namespace SocialApp.Domain.Exceptions;

public class ModelInvalidException : Exception
{
    public string[] ValidationErrors { get; private set; }

    public ModelInvalidException(string message, string[] errors)
		: base(message)
	{
        ValidationErrors = errors;
    }
}
