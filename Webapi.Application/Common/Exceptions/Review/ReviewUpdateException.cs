using System.Net;

namespace Webapi.Application.Common.Exceptions.Review;

public class ReviewUpdateException : BadRequestException
{
    public ReviewUpdateException(Guid id, string message)
        : base($"Error updating review with ID {id}: {message}")
    {
    }

    public ReviewUpdateException() :
        base("Error updating review")
    {
        
    }
}