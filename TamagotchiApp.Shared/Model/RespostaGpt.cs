namespace TamagotchiApp.Shared.Model;

public class RespostaGpt
{
    public string Mensagem { get; set; } = string.Empty;
    public int Humor { get; set; }
    public int Fome { get; set; }
    public int Sono { get; set; }
}
