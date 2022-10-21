using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Acme.MyConsoleApp;

public class HelloWorldService : ITransientDependency
{
    public ILogger<HelloWorldService> Logger { get; set; }
    private readonly TestClient _client;
    private readonly IConfiguration _configuration;

    public HelloWorldService(TestClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
        Logger = NullLogger<HelloWorldService>.Instance;
    }

    public async Task SayHelloAsync()
    {
        Logger.LogInformation("Hello World!");
        var connectionString = _configuration.GetConnectionString("Default");
        using var conn = new SqlConnection(connectionString);
        var sql = "SELECT TOP 5 * FROM AppOrders";
        var results = conn.Query<Order>(sql).ToList();
        Logger.LogDebug("{@Orders}",results);
        var orderNumber = results.FirstOrDefault()?.OrderNumber;
        Logger.LogDebug("Number: {@Number}",orderNumber);
        var response = await _client.GetAccountAsync(orderNumber);
        Logger.LogDebug("Response: {@Response}",response);
        Logger.LogInformation("Finish {@Method}!",nameof(SayHelloAsync));
    }
}