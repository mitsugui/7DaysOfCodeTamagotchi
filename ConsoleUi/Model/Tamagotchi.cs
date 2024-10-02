
namespace ConsoleUi.Model;

public class Tamagotchi
{
    public int Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string[]? Habilidades { get; init; }
    public string[]? Tipos { get; init; }
    public int Altura { get; init; }
    public int Peso { get; init; }
    public int Humor { get; internal set; }
    public int Fome { get; internal set; }
    public int Sono { get; internal set; }
    public string Emoji { get; internal set; } = string.Empty;
    public string Mensagem { get; internal set; } = string.Empty;

    public DateTimeOffset? HorarioDormiu { get; private set; }
    public bool EstaDormindo { get => HorarioDormiu != null; }

    public string StatusFome
    {
        get => Fome switch
        {
            < 1 => $"{Nome} comeu demais.",
            < 6 => $"{Nome} está satisfeito.",
            < 9 => $"{Nome} está com fome.",
            _ => $"{Nome} com muita fome."
        };
    }

    public string StatusHumor
    {
        get => Humor switch
        {
            < 2 => $"{Nome} está zangado.",
            < 5 => $"{Nome} está mal humorado.",
            < 9 => $"{Nome} está bem humorado.",
            _ => $"{Nome} está muito feliz."
        };
    }

    public string StatusSono
    {
        get
        {
            return Sono switch
            {
                < 3 => $"{Nome} dormiu bem.",
                < 6 => $"{Nome} está com sono.",
                _ => $"{Nome} está desmaiando de sono."
            };
        }
    }

    public void Mascote()
    {
        //Inicializa com valores aleatórios entre 1 e 10
        var rand = new Random();
        Fome = rand.Next(1, 10);
        Humor = rand.Next(1, 10);
        Sono = rand.Next(1, 10);
    }

    public void DormirOuAcordar()
    {
        HorarioDormiu = HorarioDormiu == null ? DateTimeOffset.Now : null;
    }
}