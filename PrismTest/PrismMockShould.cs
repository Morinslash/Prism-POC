using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace PrismTest;

public class PrismMockShould
{
    private readonly string testHost = "http://localhost:4010";
    [Fact]
    public async Task Test1()
    {
        string resourceId = "1";
        IHttpClientFactory testClientFactory = new TestHttpClientFactory(resourceId, testHost);

        var sut = new GreetingClientApi(testClientFactory);
        var httpResponse = await sut.GetGreetingWith(resourceId);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var resultRecord = JsonConvert.DeserializeObject<GreetingRecord>(responseContent);

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
        string resourceId = "2";
        IHttpClientFactory testClientFactory = new TestHttpClientFactory(resourceId, testHost);
        
        var sut = new GreetingClientApi(testClientFactory);
        var httpResponse = await sut.GetGreetingWith(resourceId);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var resultRecord = JsonConvert.DeserializeObject<GreetingRecord>(responseContent);

        var expectedRecord = new GreetingRecord
        {
            GreetingId = 2,
            Greeting = "Good",
            DayTime = "Day!!"
        };
        Assert.Equal(expectedRecord, resultRecord);
    }
}