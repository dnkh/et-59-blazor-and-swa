using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyPOIs.Client;
using MyPOIs.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("MyPOIs.LocalAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "/sample-data/"));
builder.Services.AddScoped<IPOIDataService, POIDataServiceLocal>();

await builder.Build().RunAsync();
