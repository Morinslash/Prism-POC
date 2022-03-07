using System.Net.Http;
using System.Threading.Tasks;

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
}