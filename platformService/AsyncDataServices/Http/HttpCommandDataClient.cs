using platformService.Dtos;
using System.Text;
using System.Text.Json;

namespace platformService.SyncDataServices.Http;

public class HttpCommandDataClient(HttpClient client, IConfiguration configuration) : ICommandDataClient
{
    private readonly HttpClient _httpClient = client;
    private readonly IConfiguration _configuration = configuration;

    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platformReadDto),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync($"{ _configuration["RelatedServices:CommandService"]! }/platforms", httpContent);

        if (response.IsSuccessStatusCode)
            Console.WriteLine("--> Sync POST to CommandService was OK!");
        else
            Console.WriteLine("--> Sync POST to CommandService failded!");
    }
}
