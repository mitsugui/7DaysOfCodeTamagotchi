using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TamagotchiApp.Shared.Service;
using TamagotchiApp.Shared.Data;
using TamagotchiWasmApp;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<TamagotchiService>();
builder.Services.AddScoped<GptChatCompletionService>();
builder.Services.AddScoped<RepositorioMascotes>();
builder.Services.AddTransient<IPlayerStateStorage, PlayerStateLocalStorage>();

await builder.Build().RunAsync();
