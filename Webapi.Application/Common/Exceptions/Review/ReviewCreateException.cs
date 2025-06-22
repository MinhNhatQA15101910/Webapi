using System.Net;

namespace Webapi.Application.Common.Exceptions.Review;

public class ReviewCreateException : BadRequestException
{
    public ReviewCreateException(string message)
        : base( $"Error creating review: {message}")
    {
    }
    public ReviewCreateException() : base( "Error creating review." ) {}
}