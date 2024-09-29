
namespace ConsoleUi.Model;

public class Tamagotchi
{
    private int _fome;
    private int _humor;
    private int _sono;
    private DateTime? _horarioDormiu;

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
            < 1 => $"{Nome} comeu demais.",
            < 6 => $"{Nome} está satisfeito.",
            < 9 => $"{Nome} está com fome.",
            _ => $"{Nome} com muita fome."
        };
    }

    public string StatusHumor
    {
        get => _humor switch
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
            AtualizarSono();
            return _sono switch
            {
                < 3 => $"{Nome} dormiu bem.",
                < 6 => $"{Nome} está com sono.",
                _ => $"{Nome} está desmaiando de sono."
            };
        }
    }

    public bool EstaDormindo => _horarioDormiu != null;

    public void Mascote()
    {
        //Inicializa com valores aleatórios entre 1 e 10
        var rand = new Random();
        _fome = rand.Next(1, 10);
        _humor = rand.Next(1, 10);
        _sono = rand.Next(1, 10);
    }

    public void Alimentar()
    {
        if (EstaDormindo) return;

        if (_fome > 0) _fome -= 1;
        _humor = Math.Min(10, _humor + 1);
    }

    public void Brincar()
    {
        if (EstaDormindo) return;

        _fome = Math.Min(10, _fome + 1);
        _sono = Math.Min(10, _sono + 2);
        _humor = Math.Min(10, _humor + 4);
    }

    public void Dormir()
    {
        _horarioDormiu = DateTime.Now;
    }

    public void Acordar()
    {
        AtualizarSono();
        _horarioDormiu = null;
    }



    private void AtualizarSono()
    {
        if (_horarioDormiu == null) return;

        const int totalPrecisaDormir = 5 * 60;
        var segundosDormidos = (DateTime.Now - _horarioDormiu.Value).TotalSeconds;
        var reducaoSono = segundosDormidos >= totalPrecisaDormir ? 10 : 10 * segundosDormidos / totalPrecisaDormir;

        _sono = Math.Max(0, _sono - (int)reducaoSono);
    }
}