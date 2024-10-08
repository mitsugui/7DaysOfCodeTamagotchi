using TamagotchiApp.ConsoleUi.Views;
using TamagotchiApp.ConsoleUi.Controller;
using TamagotchiApp.Shared.Service;
using Microsoft.Extensions.DependencyInjection;


var serviceCollection = new ServiceCollection()
    .AddTransient<TamagotchiService>()
    .AddTransient<GptChatCompletionService>();

serviceCollection.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://192.168.31.184:5218");
});

serviceCollection.AddHttpClient("PokemonAPI", client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var controller = new TamagotchiController(
    new TamagotchiView(), 
    serviceProvider.GetService<TamagotchiService>()!,
    serviceProvider.GetService<GptChatCompletionService>()!);
await controller.JogarAsync();

return;