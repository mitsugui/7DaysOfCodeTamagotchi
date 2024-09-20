public record PokemonResponse
(
    int Id,
    string Name,
    PokemonAbilitiesResponse[] Abilities,
    PokemonTypesResponse[] Types,
    int Height,
    int Weight
);
