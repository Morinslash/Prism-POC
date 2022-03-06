using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace PrismTest;

public class PrismMockShould
{
    [Fact]
    public async Task Test1()
    {
        int id = 1;
        HttpClient testClient = new HttpClient();
        testClient.DefaultRequestHeaders.Add("Prefer",$"example={id}");
       
        var sut = new GreetingClientApi(testClient);
        var result = await sut.GetGreetingWith(id);
        var greeting = await result.Content.ReadAsStringAsync();
        var resultRecord = JsonConvert.DeserializeObject<GreetingRecord>(greeting);

        var expectedRecord = new GreetingRecord
        {
            GreetingId = 1,
            Greeting = "Hello",
            DayTime = "World!!"
        };
        Assert.Equal(expectedRecord, resultRecord);
    }
    
    [Fact]
    public async Task Test2()
    {
        int id = 2;
        HttpClient testClient = new HttpClient();
        testClient.DefaultRequestHeaders.Add("Prefer",$"example={id}");
        
        var sut = new GreetingClientApi(testClient);
        var result = await sut.GetGreetingWith(id);
        var greeting = await result.Content.ReadAsStringAsync();
        var resultRecord = JsonConvert.DeserializeObject<GreetingRecord>(greeting);

        var expectedRecord = new GreetingRecord
        {
            GreetingId = 2,
            Greeting = "Good",
            DayTime = "Day!!"
        };
        Assert.Equal(expectedRecord, resultRecord);
    }
}

public record GreetingRecord
{
    public int GreetingId { get; init; }
    public string Greeting { get; init; }
    public string DayTime { get; init; }
}

public class GreetingClientApi
{
    private readonly HttpClient _httpClient;

    public GreetingClientApi(HttpClient client)
    {
        _httpClient = client;
    }
    public async Task<HttpResponseMessage> GetGreetingWith(int id)
    {
        var result = await _httpClient.GetAsync($"http://localhost:4010/hello-world/{id}");
        return result;
    }
}