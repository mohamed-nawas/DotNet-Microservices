using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("Command: " + nameof(command) + " is null");
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException("Platform: " + nameof(platform) + " is null");
            }
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
            .Where(c => c.Id == commandId && c.PlatformId == platformId)
            // .Where(c => c.PlatformId == platformId)
            .FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool saveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}