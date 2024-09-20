using RestSharp;

const string BaseUrl = @"https://pokeapi.co/api/v2/";


var client = new RestClient(BaseUrl);

var pokemons = new[]
{
    (Nome: "Pikachu", Url: "pokemon/25/"),
    (Nome: "Bulbasaur", Url: "pokemon/1/"),
    (Nome: "Charmander", Url: "pokemon/4/"),
    (Nome: "Ivysaur", Url: "pokemon/2/"),
    (Nome: "Pidgeot", Url: "pokemon/18/"),
    (Nome: "Psyduck", Url: "pokemon/54/"),
    (Nome: "Squirtle", Url: "pokemon/7/"),
};

var indiceEscolha = -1;
while (indiceEscolha == -1)
{
    Console.WriteLine("Escolha o pokemon que deseja detalhar:");
    for (var i = 0; i < pokemons.Length; i++)
    {
        Console.WriteLine($"{i + 1} - {pokemons[i].Nome}");
    }

    if (!int.TryParse(Console.ReadLine(), out indiceEscolha)
        || indiceEscolha < 1
        || indiceEscolha > pokemons.Length)
    {
        Console.WriteLine("Escolha inválida");
        indiceEscolha = -1;
    }
}

var escolha = pokemons[indiceEscolha - 1];

var request = new RestRequest(escolha.Url);
var pokemon = await client.GetAsync<PokemonResponse>(request, CancellationToken.None);

Console.WriteLine($@"Pokemon: {pokemon?.Name} ({BaseUrl}/{escolha.Url})");

Console.WriteLine($" Altura: {pokemon?.Height}");
Console.WriteLine($" Peso: {pokemon?.Weight}");
if (pokemon?.Types != null)
{
    Console.WriteLine($" Tipos:");
    foreach (var tipo in pokemon.Types)
    {
        Console.WriteLine($" - {tipo.Type.Name}");
    }
}
if (pokemon?.Abilities != null)
{
    Console.WriteLine($" Habilidades:");
    foreach (var ability in pokemon.Abilities)
    {
        Console.WriteLine($" - {ability.Ability.Name}");
    }
}

Console.ReadKey();