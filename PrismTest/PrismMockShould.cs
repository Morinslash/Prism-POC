using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Moq;
using Moq.Protected;
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
            greeting = "Hello",
            dayTime = "World!!"
        };
        Assert.Equal(expectedRecord, resultRecord);
    }
    
    [Fact]
    public async Task Test2()
    {
        string resourceId = "2";
        IHttpClientFactory testClientFactory = new TestHttpClientFactory(resourceId, testHost);
        var clinet = new Mock<HttpClient>();
        HttpRequestMessage expected;
        var sut = new GreetingClientApi(testClientFactory);
        var httpResponse = await sut.GetGreetingWith(resourceId);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var resultRecord = JsonConvert.DeserializeObject<GreetingRecord>(responseContent);

        var expectedRecord = new GreetingRecord
        {
            GreetingId = 2,
            greeting = "Good",
            dayTime = "Day!!"
        };
        Assert.Equal(expectedRecord, resultRecord);
    }

    [Fact]
    public async Task CaptureResponse()
    {
        string resourceId = "1";
        IHttpClientFactory testClientFactory = new TestHttpClientFactory(resourceId, testHost);
        var mockClient = new HttpClientSpy();
        mockClient.BaseAddress = new Uri(testHost);
        
        var greetingClientApi = new GreetingClientApi(testClientFactory);
        await greetingClientApi.PostGreeting(resourceId, mockClient);
        await greetingClientApi.PostGreeting(resourceId, mockClient);
        await greetingClientApi.PostGreeting(resourceId, mockClient);
        var httpResponseMessage = await greetingClientApi.PostGreeting(resourceId, mockClient);
        var httpRequestMessage = mockClient.captor.First();
        var content = await httpRequestMessage.Content.ReadAsStringAsync();
        Assert.True(httpResponseMessage.StatusCode == HttpStatusCode.Created);
    }
}

public class HttpClientSpy : HttpClient
{
    public List<HttpRequestMessage> captor = new();
    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        captor.Add(request);
        return base.SendAsync(request);
    }
}