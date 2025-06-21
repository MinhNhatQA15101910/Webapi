namespace Webapi.Application.Common.Exceptions.Category;

public class CategoryDeleteException : BadRequestException
{
    public CategoryDeleteException(Guid categoryId, string message)
        : base($"Failed to delete category {categoryId}: {message}")
    {
    }

    public CategoryDeleteException(Guid categoryId)
        : base($"An error occurred while deleting category {categoryId}.")
    {
    }
}