using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Covauto.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

    // port
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5095/") });
await builder.Build().RunAsync();