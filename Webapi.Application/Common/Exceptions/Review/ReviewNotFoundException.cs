using System.Net;

namespace Webapi.Application.Common.Exceptions.Review;

public class ReviewNotFoundException : BadRequestException
{
    public ReviewNotFoundException(Guid id)
        : base( $"Review with ID {id} not found")
    {
    }

    public ReviewNotFoundException() 
    : base( "Review not found." )
    {
        
    }
}