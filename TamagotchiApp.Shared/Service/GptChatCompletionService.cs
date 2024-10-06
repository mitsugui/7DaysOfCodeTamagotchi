using System.Net.Http.Json;
using TamagotchiApp.Shared.Model;

namespace TamagotchiApp.Shared.Service;

public class GptChatCompletionService
{
    private readonly HttpClient _httpClient;

    public GptChatCompletionService()
    {
        _httpClient = new HttpClient
		{
			BaseAddress = new Uri("http://localhost:5218")
		};
    }

    public async Task GetGpt4CompletionAsync(Tamagotchi tamagotchi, PromptAcao prompt)
    {
		List<HistoricoItem> historico = new();
		if (tamagotchi.Historico.Count > 1)
		{
			var ultimosRegistros = tamagotchi.Historico
				.OrderByDescending(x => x.Horario)
				.Take(5)
				.ToList();

			for (var i = 0; i < ultimosRegistros.Count - 1; i++)
			{
				var registro = ultimosRegistros[i];
				var registroAnterior = ultimosRegistros[i + 1];
				historico.Add(new HistoricoItem
				{
					Prompt = new PromptAcao
					{
						Acao = registro.Acao,
						Humor = registroAnterior.Humor,
						Fome = registroAnterior.Fome,
						Sono = registroAnterior.Sono,
						EstaDormindo = registroAnterior.EstaDormindo
					},
					Resposta =new RespostaGpt
					{
						Mensagem = registro.Mensagem,
						Humor = registro.Humor,
						Fome = registro.Fome,
						Sono = registro.Sono
					}
				});
			}
		}

        var request = new PromptRequest
        {
            Prompt = prompt,
            Historico = historico
        };

        var response = await _httpClient.PostAsJsonAsync("api/gpt/interact", request);
        if (response.IsSuccessStatusCode)
        {
            var respostaGpt = await response.Content.ReadFromJsonAsync<RespostaGpt>();
            if (respostaGpt == null) return;

			tamagotchi.Mensagem = respostaGpt.Mensagem;
			tamagotchi.Humor = respostaGpt.Humor;
			tamagotchi.Fome = respostaGpt.Fome;
			tamagotchi.Sono = respostaGpt.Sono;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error calling API: {response.ReasonPhrase}, Details: {errorContent}");
        }
    }
}
