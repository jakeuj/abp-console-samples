using System.Net.Http;
using System.Threading.Tasks;

namespace Acme.MyConsoleApp;

public class TestClient
{
    private readonly HttpClient _client;
    public TestClient(HttpClient client)
    {
        _client = client;
    }
    public async Task<string>GetAccountAsync(string name)
    {
        var response = await _client.GetAsync($"/api/abp/multi-tenancy/tenants/by-name/{name}");
        return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : null;
    }
}