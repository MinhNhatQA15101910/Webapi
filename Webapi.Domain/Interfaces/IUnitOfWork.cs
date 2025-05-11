namespace Webapi.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    Task<bool> CompleteAsync();
}
