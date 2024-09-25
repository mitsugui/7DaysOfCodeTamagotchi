namespace ConsoleUi.Model;

public record Pokemon
(
    int Id,
    string Name,
    Abilities[] Abilities,
    PokemonTypes[] Types,
    int Height,
    int Weight
);
