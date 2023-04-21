using BlazorApp1.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp1.Client.Providers;
using BlazorApp1.Client.State;
using Blazored.LocalStorage;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using BlazorApp1.Shared;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    //config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 200;
}
);

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    return client;
});
//builder.Services
//    .AddGrpcClient<WeatherForecast..GreeterClient>(options =>
//    {
//        options.Address = new Uri("https://localhost:5001");
//    })
//    .ConfigurePrimaryHttpMessageHandler(
//        () => new GrpcWebHandler(new HttpClientHandler()));
builder.Services.AddSingleton(services =>
{
    // Create a gRPC-Web channel pointing to the backend server
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
    var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });

    // Now we can instantiate gRPC clients for this channel
    return new WeatherForecasts.WeatherForecastsClient(channel);
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserState>();
builder.Services
.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


await builder.Build().RunAsync();