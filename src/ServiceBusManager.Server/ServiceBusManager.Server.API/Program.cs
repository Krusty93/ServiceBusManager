using Azure.Identity;
using ServiceBusManager.Server.API;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVault:Name"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

WebApplication app = builder.Build();

startup.Configure(app);

app.Run();
