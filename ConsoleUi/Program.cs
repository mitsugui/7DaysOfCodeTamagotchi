using ConsoleUi.Views;
using ConsoleUi.Service;
using ConsoleUi.Controller;


var controller = new TamagotchiController(new TamagotchiView(), new TamagotchiService());
await controller.JogarAsync();

return;