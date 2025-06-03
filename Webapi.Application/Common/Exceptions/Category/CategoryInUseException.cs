namespace Webapi.Application.Common.Exceptions.Category;

public class CategoryInUseException : BadRequestException
{
    public CategoryInUseException(Guid categoryId)
        : base($"Category {categoryId} cannot be deleted because it is associated with one or more products.")
    {
    }
    
    public CategoryInUseException(string message)
        : base(message)
    {
    }
}