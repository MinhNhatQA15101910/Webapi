namespace Webapi.Application.Common.Exceptions;

public class ApplicationException(string title, string message) : Exception(message)
{
    public string Title { get; } = title;
}
