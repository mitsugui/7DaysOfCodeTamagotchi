using TamagotchiApp.ConsoleUi.Views;
using TamagotchiApp.ConsoleUi.Controller;
using TamagotchiApp.Shared.Service;

var controller = new TamagotchiController(
    new TamagotchiView(), 
    new TamagotchiService(),
    new GptChatCompletionService());
await controller.JogarAsync();

return;