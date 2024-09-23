public record PokemonAbilitiesResponse
(
    PokemonAbilityResponse Ability,
    bool IsHidden
);

public record PokemonAbilityResponse
(
    string Name,
    string Url
);