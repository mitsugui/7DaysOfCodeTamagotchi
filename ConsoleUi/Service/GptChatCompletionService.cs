using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleUi.Model;

namespace ConsoleUi.Service;

public class GptChatCompletionService
{
	private const string Prompt = @"
		Você é um mascote pokemon que irá interagir com criaças entre 5 e 10 anos de idade. 
		Dados:
		1. Uma ação (brincar, alimentar, dormir, conversar, etc)
		2. Um valor de 0 a 10 de humor, sendo 0 muito mal humor e 10 muito bom humor.
		3. Um valor de 0 a 10 de fome, sendo 0 nenhuma fome e 10 muita fome.
		4. Um valor de 0 a 10 de sono, sendo 0 nenhum sono e 10 muito sono.
		Em formato json, como, por exemplo, {""Acao"":""brincar"", ""Humor"":5, ""Fome"":3, ""Sono"":7}.
		Devolva uma mensagem de resposta à ação e novos valores de humor, fome, sono baseado na interação realizada.
		Na mensagem de resposta, tente ser pró-ativo e sugira uma próxima ação de acordo com o humor, sono e fome do mascote.
		Ações como brincar, correr, pular, etc melhoram o humor mas aumentam a fome e o sono também. 
		Ações como dormir, tirar soneca, etc, diminuem o sono, melhoram um pouco o humor e aumentam um pouco a fome. 
		Ações com comer, se alimetar, etc, diminuem a fome e melhoram o humor. 
		Ações como dar bronca, pioram o humor.
		A resposta deve ser um json válido com caracteres de escaping caso necessário.

		Exemplos:
		Ação: {""Acao"":""brincar"", ""Humor"":5, ""Fome"":3, ""Sono"":7}
		Resposta: {""Mensagem"":""Eba!! Que divertido!"", ""Humor"":7, ""Fome"":5, ""Sono"":9}

		Ação: {""Acao"":""dormir"", ""Humor"":7, ""Fome"":5, ""Sono"":9}
		Resposta: {""Mensagem"":""Ai que sono!!"", ""Humor"":8, ""Fome"":6, ""Sono"":4}";

	private readonly string _model;
	private readonly string _organization;
	private readonly string _projectId;
	private readonly string _apiKey;
	private readonly HttpClient _httpClient = new();

	public GptChatCompletionService()
	{
		_apiKey = Environment.GetEnvironmentVariable("API_KEY")!;
		_organization = Environment.GetEnvironmentVariable("API_ORGANIZATION")!;
		_projectId = Environment.GetEnvironmentVariable("API_PROJECT_ID")!;
		_model = "gpt-4o-mini";

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
		_httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", _organization);
		_httpClient.DefaultRequestHeaders.Add("OpenAI-Project", _projectId);
	}

	public async Task GetGpt4CompletionAsync(Tamagotchi tamagotchi, PromptAcao prompt)
	{
		List<GptMessage> messages =
		[
			new GptMessage
			{
				Role = "system",
				Content = Prompt
			}
		];

		if (tamagotchi.Historico.Count > 0)
		{
			var ultimosRegistros = tamagotchi.Historico
				.OrderByDescending(x => x.Horario)
				.Take(5)
				.ToList();

			for (var i = 0; i < ultimosRegistros.Count - 1; i++)
			{
				var registro = ultimosRegistros[i];
				var registroAnterior = ultimosRegistros[i + 1];
				messages.Add(new GptMessage
				{
					Role = "user",
					Content = new PromptAcao
					{
						Acao = registro.Acao,
						Humor = registroAnterior.Humor,
						Fome = registroAnterior.Fome,
						Sono = registroAnterior.Sono,
						EstaDormindo = registroAnterior.EstaDormindo
					}.ToJson()
				});
				messages.Add(new GptMessage
				{
					Role = "assistant",
					Content = new RespostaGpt
					{
						Mensagem = registro.Mensagem,
						Humor = registro.Humor,
						Fome = registro.Fome,
						Sono = registro.Sono
					}.ToJson()
				});
			}
		}

		messages.Add(new GptMessage
		{
			Role = "user",
			Content = prompt.ToJson()
		});

		var apiUrl = "https://api.openai.com/v1/chat/completions";
		var requestBody = JsonSerializer.Serialize(new GptRequest
		{
			Model = _model,
			Messages = messages.ToArray(),
		});

		using var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
		using var responseMessage = await _httpClient.PostAsync(apiUrl, content);

		if (!responseMessage.IsSuccessStatusCode)
		{
			var errorContent = await responseMessage.Content.ReadAsStringAsync();
			throw new Exception($"Error calling GPTAPI: {responseMessage.ReasonPhrase}, Detalehs: {errorContent}");
		}

		var responseBody = await responseMessage.Content.ReadAsStringAsync();
		var root = JsonSerializer.Deserialize<JsonElement>(responseBody);

		var choices = root.GetProperty("choices");
		var message = choices.EnumerateArray().First()
			.GetProperty("message");
		var conteudoResposta = message.GetProperty("content")
			.GetString() ?? "";

		var respostaGpt = JsonSerializer.Deserialize<RespostaGpt>(conteudoResposta)
			?? throw new Exception("Erro ao deserializar resposta do GPT");
			
        tamagotchi.Mensagem = respostaGpt.Mensagem;
		tamagotchi.Humor = respostaGpt.Humor;
		tamagotchi.Fome = respostaGpt.Fome;
		tamagotchi.Sono = respostaGpt.Sono;
	}
}

public class PromptAcao
{
	public string? Acao { get; set; }
	public int Humor { get; set; }
	public int Fome { get; set; }
	public int Sono { get; set; }
	public bool EstaDormindo { get; set; }
}

public class RespostaGpt
{
	public string Mensagem { get; set; } = string.Empty;
	public int Humor { get; set; }
	public int Fome { get; set; }
	public int Sono { get; set; }
}

public class GptMessage
{
	[JsonPropertyName("role")]
	public string Role { get; init; } = "user";

	[JsonPropertyName("content")]
	public string? Content { get; init; }
}

public class GptRequest
{
	[JsonPropertyName("model")]
	public string Model { get; init; } = "gpt-4";

	[JsonPropertyName("messages")]
	public GptMessage[]? Messages { get; set; }

	[JsonPropertyName("max_tokens")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? MaxTokens { get; init; }

	[JsonPropertyName("n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? N { get; init; }

	[JsonPropertyName("stop")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Stop { get; init; }

	[JsonPropertyName("temperature")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public double? Temperature { get; init; }
}