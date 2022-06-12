using Client;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(services =>
{
	var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
	var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
	var channel = GrpcChannel.ForAddress("https://localhost:7159", new GrpcChannelOptions { HttpClient = httpClient });
	return new Restaurant.RestaurantClient(channel);
});


await builder.Build().RunAsync();
