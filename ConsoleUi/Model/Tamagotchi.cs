namespace ConsoleUi.Model;

public class Tamagotchi
{
    public int Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string[]? Habilidades { get; init; }
    public string[]? Tipos { get; init; }
    public int Altura { get; init; }
    public int Peso { get; init; }
}