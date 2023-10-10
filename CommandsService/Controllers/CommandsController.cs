using System.ComponentModel.Design;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/v1/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine("Getting commands from commandsservice");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItems = _repository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine("Getting command from commandsservice");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItem = _repository.GetCommand(platformId, commandId);
            if (commandItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto dto)
        {
            Console.WriteLine("Creating command for platform in commandsservice");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(dto);
            _repository.CreateCommand(platformId, command);
            _repository.saveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}