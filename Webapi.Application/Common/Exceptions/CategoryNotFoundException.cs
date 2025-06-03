namespace Webapi.Application.Common.Exceptions;

public class CategoryNotFoundException(Guid categoryId)
    : NotFoundException($"The category with the identifier {categoryId} was not found.")
{
}
