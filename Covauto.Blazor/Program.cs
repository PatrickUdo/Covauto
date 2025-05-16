using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Covauto.Blazor;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };

    return httpClient;
});

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5095/")
}
.EnableIntercept(sp)
.EnableCookies());

await builder.Build().RunAsync();

public static class HttpClientExtensions
{
    public static HttpClient EnableCookies(this HttpClient client)
    {
        client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        return client;
    }

    public static HttpClient EnableIntercept(this HttpClient client, IServiceProvider services)
    {
        client.DefaultRequestHeaders.Add("Access-Control-Allow-Credentials", "true");
        return client;
    }
}