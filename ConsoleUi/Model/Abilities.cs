namespace ConsoleUi.Model;

public record Abilities
(
    Ability Ability,
    bool IsHidden
);

public record Ability
(
    string Name,
    string Url
);