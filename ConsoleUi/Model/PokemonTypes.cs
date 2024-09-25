namespace ConsoleUi.Model;

public record PokemonTypes
(
    PokemonTypeResponse Type
);

public record PokemonTypeResponse
(
    string Name,
    string Url
);