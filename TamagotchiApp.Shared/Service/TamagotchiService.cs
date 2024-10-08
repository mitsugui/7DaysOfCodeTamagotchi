using System.Text.Json;
using TamagotchiApp.Shared.Model;
using TamagotchiApp.Shared.Utils;

namespace TamagotchiApp.Shared.Service;

public class TamagotchiService
{
    private static readonly Dictionary<string, InfoMascote> Pokemons = new()
    {
        {"Pikachu", new InfoMascote("Pikachu", "pokemon/25/", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png")},
        {"Bulbasaur", new InfoMascote("Bulbasaur", "pokemon/1/", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png")},
        {"Charmander", new InfoMascote("Charmander", "pokemon/4/", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/4.png")},
        {"Ivysaur", new InfoMascote("Ivysaur", "pokemon/2/", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/2.png")},
        {"Pidgeot", new InfoMascote("Pidgeot", "pokemon/18/", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/18.png")},
        {"Psyduck", new InfoMascote("Psyduck", "pokemon/54/", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/54.png")},
        {"Squirtle", new InfoMascote("Squirtle", "pokemon/7/", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/7.png")},
    };

    private readonly HttpClient _client;

    public TamagotchiService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("PokemonAPI");
    }

    public IReadOnlyCollection<InfoMascote> ListarMascotes()
    {
        return Pokemons.Values;
    }

    public InfoMascote? ObterInfoMascote(string pokemon)
    {
        return Pokemons.TryGetValue(pokemon, out var info) ? info : null;
    }

    public async Task<Tamagotchi?> ObterMascoteAsync(string url)
    {
        var textoJson = await _client.GetStringAsync(url);

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        var pokemon = JsonSerializer.Deserialize<Pokemon>(textoJson, serializeOptions);
        if (pokemon == null) return null;

        return new Tamagotchi
        {
            Id = pokemon.Id,
            Nome = pokemon.Name.ToPascalCase(),
            Url = url,
            ImageUrl = pokemon.Sprites.FrontDefault,
            Especie = pokemon.Name.ToPascalCase(),
            Habilidades = pokemon.Abilities
                .Select(a => a.Ability.Name.ToPascalCase())
                .ToArray(),
            Tipos = pokemon.Types
                .Select(t => t.Type.Name.ToPascalCase())
                .ToArray(),
            Altura = pokemon.Height,
            Peso = pokemon.Weight
        };
    }
}

internal record Pokemon
(
    int Id,
    string Name,
    Abilities[] Abilities,
    PokemonTypes[] Types,
    PokemonSprites Sprites,
    int Height,
    int Weight
);

internal record PokemonTypes
(
    PokemonTypeResponse Type
);

internal record PokemonTypeResponse
(
    string Name,
    string Url
);

internal record Abilities
(
    Ability Ability,
    bool IsHidden
);

internal record Ability
(
    string Name,
    string Url
);

internal record PokemonSprites
(
    string FrontDefault
);