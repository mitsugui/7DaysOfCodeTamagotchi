namespace ConsoleUi.Model;

public class Tamagotchi
{
    private int _fome;
    private int _humor;
    private int _sono;

    public int Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string[]? Habilidades { get; init; }
    public string[]? Tipos { get; init; }
    public int Altura { get; init; }
    public int Peso { get; init; }

    public string StatusFome
    {
        get => _fome switch
        {
            < 3 => $"{Nome} com muita fome.",
            < 6 => $"{Nome} está com fome.",
            < 9 => $"{Nome} está satisfeito.",
            _ => $"{Nome} comeu demais."
        };
    }

    public string StatusHumor
    {
        get => _humor switch
        {
            < 3 => $"{Nome} está zangado.",
            < 6 => $"{Nome} está mal humorado.",
            < 9 => $"{Nome} está bem humorado.",
            _ => $"{Nome} está muito feliz."
        };
    }

    public string StatusSono
    {
        get => _sono switch
        {
            < 3 => $"{Nome} está desmaiando de sono.",
            < 6 => $"{Nome} está com sono.",
            _ => $"{Nome} dormiu bem."
        };
    }

    public void Mascote()
    {
        //Inicializa com valores aleatórios entre 1 e 10
        _fome = 5;
        _humor = 3;
        _sono = 5;
    }
}