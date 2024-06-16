using ATMS.Web.BlazarClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string apiEndPoint = "https://localhost:7273";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiEndPoint) });

await builder.Build().RunAsync();
