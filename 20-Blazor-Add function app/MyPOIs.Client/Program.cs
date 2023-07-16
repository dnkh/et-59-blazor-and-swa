using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyPOIs.Client;
using MyPOIs.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUri = builder.Configuration.GetValue<string>("MyPOIsAPIUrl");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("MyPOIs.LocalAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "/sample-data/"));
builder.Services.AddHttpClient("MyPOIs.RemoteAPI", client => client.BaseAddress = new Uri(apiUri));
builder.Services.AddScoped<IPOIDataService, POIDataServiceRemote>();


await builder.Build().RunAsync();
