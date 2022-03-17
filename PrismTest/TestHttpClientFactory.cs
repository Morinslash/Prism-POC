using System;
using System.Net.Http;

namespace PrismTest;

public class TestHttpClientFactory : IHttpClientFactory
{
    private readonly string _testHeaderValue;
    private string _testHost;

    public TestHttpClientFactory(string testHeaderValue, string testHost)
    {
        _testHeaderValue = testHeaderValue;
        _testHost = testHost;
    }
    public HttpClient CreateClient(string name)
    {
        var httpClient = new HttpClientSpy();
        httpClient.BaseAddress = new Uri(_testHost);
        httpClient.DefaultRequestHeaders.Add("Prefer",$"example={_testHeaderValue}");
        return httpClient;
    }
}