using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Data;
using AutoMapper;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlatformsController : ControllerBase
    {

        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _client;

        public PlatformsController(IPlatformRepository repository, IMapper mapper, ICommandDataClient client)
        {
            _repository = repository;
            _mapper = mapper;
            _client = client;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("Getting Platforms");
            var platformItems = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatform")]
        public ActionResult<PlatformReadDto> GetPlatform(int id)
        {
            Console.WriteLine("Getting platform");
            var platformItem = _repository.GetPlatformById(id);
            if (platformItem == null)
                return NotFound("Platform for given id does not exist");
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto dto)
        {
            Console.WriteLine("Creating a platform");
            var platformModel = _mapper.Map<Platform>(dto);
            var platformItem = _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            // send an async request to command service to notify platform service created a platform
            try
            {
                await _client.SendPlatformToCommand(_mapper.Map<PlatformReadDto>(platformItem));
            }
            catch (Exception e)
            {

                Console.WriteLine($"could not send platformtocommand, exception: {e.Message}");
            }

            return Created(Url.Action("GetPlatform", new { id = platformItem.Id }), _mapper.Map<PlatformReadDto>(platformItem));
        }
    }
}