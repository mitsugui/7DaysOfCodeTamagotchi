using RestSharp;
using ConsoleUi.Model;

namespace ConsoleUi.Service;

internal class TamagotchiService
{
    private const string BaseUrl = @"https://pokeapi.co/api/v2/";

    private readonly RestClient _client = new(BaseUrl);

    public async Task<Tamagotchi?> ObterMascoteAsync(string url)
    {
        var pokemon = await _client.GetAsync<Pokemon>(new RestRequest(url), CancellationToken.None);
        if (pokemon == null) return null;

        return new Tamagotchi
        {
            Id = pokemon.Id,
            Nome = pokemon.Name.ToPascalCase(),
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