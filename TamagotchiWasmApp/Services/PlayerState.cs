using TamagotchiApp.Shared.Model;

public class PlayerState
{
    public string NomeJogador { get; set; } = string.Empty;
    public List<Tamagotchi> AdoptedMascots { get; set; } = new();
}
