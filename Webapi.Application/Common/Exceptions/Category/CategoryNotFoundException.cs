namespace Webapi.Application.Common.Exceptions.Category;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(Guid categoryId)
        : base($"The category with the identifier {categoryId} was not found.")
    {
    }
    
    public CategoryNotFoundException(string message)
        : base(message)
    {
    }
}