namespace SocialApp.Application.Models;

public class Result<T>
{
    private Tuple<AppErrorCode, List<string>>? _error;
    public T? Data { get; set; }
    public Tuple<AppErrorCode, List<string>> Errors
    { 
        get
        {
            if (_error is null)
                throw new NullReferenceException($"Use {nameof(HasError)} flag to check if there are any errors");
            return _error;
        }
    }

    public bool HasError { get; private set; }

    public void AddError(AppErrorCode code, params string[] message)
    {
        HasError = true;
        _error = new Tuple<AppErrorCode, List<string>>(code, message.ToList());
    }
}
