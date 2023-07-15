namespace SocialApp.Application.Models;

public class Result<T>
{
    private Tuple<AppErrorCode, List<string>>? _error;
    private T? _data;

    public T Data 
    {
        get => _data ?? throw new NullReferenceException($"Use {nameof(HasError)} flag to check if there are any errors");
        internal set => _data = value;
    }
    public Tuple<AppErrorCode, List<string>> Errors
    { 
        get => _error ?? throw new NullReferenceException($"Use {nameof(HasError)} flag to check if there are any errors");
    }

    public bool HasError { get; private set; }

    public void AddError(AppErrorCode code, params string[] message)
    {
        HasError = true;
        _error = new Tuple<AppErrorCode, List<string>>(code, message.ToList());
    }
}
