using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Acme.MyConsoleApp;

[DependsOn(
    typeof(AbpAutofacModule)
)]
public class MyConsoleAppModule : AbpModule
{
    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<MyConsoleAppModule>>();
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        logger.LogInformation($"MySettingName => {configuration["MySettingName"]}");

        var hostEnvironment = context.ServiceProvider.GetRequiredService<IHostEnvironment>();
        logger.LogInformation($"EnvironmentName => {hostEnvironment.EnvironmentName}");

        return Task.CompletedTask;
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ConfigureHttpClient(context, configuration);
    }
    
    private void ConfigureHttpClient(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddHttpClient<TestClient>(opt =>
        {
            if (Uri.TryCreate(configuration["BaseAddress"], UriKind.Absolute, out var uri))
            {
                opt.BaseAddress = uri;
            }
        });
    }
}
