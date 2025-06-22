using System.Net;

namespace Webapi.Application.Common.Exceptions.Review;

public class ReviewDeleteException : BadRequestException
{
    public ReviewDeleteException(Guid id, string message)
        : base( $"Error deleting review with ID {id}: {message}")
    {
    }

    public ReviewDeleteException() : base(
        $"Error deleting review with ID {Guid.Empty}")
    {
        
    }
}