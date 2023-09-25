using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly static string APPLICATION_JSON = "application/json";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto dto)
        {
            var httpContent = new StringContent(
                // serialize dto
                JsonSerializer.Serialize(dto), Encoding.UTF8, APPLICATION_JSON);

            // send async request to platform service
            string baseUrl = $"{_configuration["CommandService:BaseUrl"]}";
            string path = $"{_configuration["CommandService:TestInboundConnectionUrl"]}";
            var response = await _httpClient.PostAsync(baseUrl + path, httpContent);

            if (response.IsSuccessStatusCode) Console.WriteLine("Sync POST of platform to command service is Ok");
            else Console.WriteLine("Sync POST of platform to command service is Not Ok");
        }
    }
}