using SixMinApi.Models;

namespace SixMinApi.Data;

public interface ICommandRepository
{
    Task SaveChanges();

    Task<Command?> GetCommandById(int id);

    Task<IEnumerable<Command>> GetAllCommands();

    Task CreateCommand(Command command);

    void DeleteCommand(Command command);
}