using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrismTest;

public class GreetingClientApi
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GreetingClientApi(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }
    public async Task<HttpResponseMessage> GetGreetingWith(string id)
    {
        _httpClientFactory.CreateClient();
        var httpClient = _httpClientFactory.CreateClient();
        var result = await httpClient.GetAsync($"hello-world/{id}");
        return result;
    }

    public async Task<HttpResponseMessage> PostGreeting(string id, HttpClient client)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"hello-world/{id}");
        var greetingRecord = new GreetingRecord
        {
            greeting = "hello",
            dayTime = "world"
        };

        request.Content =
            new StringContent(JsonConvert.SerializeObject(greetingRecord), Encoding.UTF8, "application/json");
        var response = await client.SendAsync(request, CancellationToken.None);
        return response;
    }
}