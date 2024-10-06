using System.Net.Http.Json;
using TamagotchiApp.Shared.Model;
using TamagotchiApp.Shared.Utils;

namespace TamagotchiApp.Shared.Service;

public class TamagotchiService
{
    private const string BaseUrl = @"https://pokeapi.co/api/v2/";
    
    private static readonly Dictionary<string, string> Pokemons = new()
    {
        {"Pikachu", "pokemon/25/"},
        {"Bulbasaur", "pokemon/1/"},
        {"Charmander", "pokemon/4/"},
        {"Ivysaur", "pokemon/2/"},
        {"Pidgeot", "pokemon/18/"},
        {"Psyduck", "pokemon/54/"},
        {"Squirtle", "pokemon/7/"},
    };

    private readonly HttpClient _client;

    public TamagotchiService()
    {
        _client = new HttpClient
        {
            BaseAddress = new(BaseUrl)
        };
    }

    public IReadOnlyCollection<string> ListarPokemons()
    {
        return Pokemons.Keys;
    }

    public string? ObterUrlPokemon(string pokemon)
    {
        return Pokemons.TryGetValue(pokemon, out var url) ? url : null;
    } 

    public async Task<Tamagotchi?> ObterMascoteAsync(string url)
    {
        var pokemon = await _client.GetFromJsonAsync<Pokemon>(url, CancellationToken.None);
        if (pokemon == null) return null;

        return new Tamagotchi
        {
            Id = pokemon.Id,
            Nome = pokemon.Name.ToPascalCase(),
            Url = url,
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