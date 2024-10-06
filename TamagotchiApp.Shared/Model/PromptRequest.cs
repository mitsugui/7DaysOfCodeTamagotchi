namespace TamagotchiApp.Shared.Model;

public class PromptRequest
{
    public PromptAcao Prompt { get; set; } = new PromptAcao();
    public List<HistoricoItem>? Historico { get; set; }
}

public class HistoricoItem
{
    public PromptAcao Prompt { get; set; } = new PromptAcao();
    public RespostaGpt Resposta { get; set; } = new RespostaGpt();
}
