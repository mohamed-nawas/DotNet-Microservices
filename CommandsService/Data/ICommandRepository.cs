using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepository
    {

        // Platform model related methods
        bool saveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExists(int platformId);

        // Command model related methods
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}