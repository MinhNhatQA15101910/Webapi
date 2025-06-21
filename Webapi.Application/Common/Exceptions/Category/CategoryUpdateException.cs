namespace Webapi.Application.Common.Exceptions.Category;

public class CategoryUpdateException : BadRequestException
{
    public CategoryUpdateException(Guid categoryId, string message)
        : base($"Failed to update category {categoryId}: {message}")
    {
    }

    public CategoryUpdateException(Guid categoryId)
        : base($"An error occurred while updating category {categoryId}.")
    {
    }
}