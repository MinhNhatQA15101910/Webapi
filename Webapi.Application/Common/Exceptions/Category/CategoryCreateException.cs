namespace Webapi.Application.Common.Exceptions.Category;

public class CategoryCreateException : BadRequestException
{
    public CategoryCreateException(string message)
        : base($"Failed to create category: {message}")
    {
    }

    public CategoryCreateException()
        : base("An error occurred while creating the category.")
    {
    }
}