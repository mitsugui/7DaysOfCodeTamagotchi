using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TamagotchiApi.Model;
using TamagotchiApp.Shared.Model;

namespace TamagotchiApi.Service;

public class OpenAiService
{
    private const string OpenAiApiUrl = "https://api.openai.com/v1/chat/completions";
    private const string Prompt = @"
Você é um mascote pokemon que irá interagir com crianças entre 5 e 10 anos de idade. 
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
Ações com comer, se alimentar, etc, diminuem a fome e melhoram o humor. 
Ações como dar bronca, pioram o humor.
A resposta deve ser um json válido com caracteres de escape caso necessário.

Exemplos:
Ação: {""Acao"":""brincar"", ""Humor"":5, ""Fome"":3, ""Sono"":7}
Resposta: {""Mensagem"":""Eba!! Que divertido!"", ""Humor"":7, ""Fome"":5, ""Sono"":9}

Ação: {""Acao"":""dormir"", ""Humor"":7, ""Fome"":5, ""Sono"":9}
Resposta: {""Mensagem"":""Ai que sono!!"", ""Humor"":8, ""Fome"":6, ""Sono"":4}";

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
	private readonly string _organization;
	private readonly string _projectId;
    private readonly string _model;

    public OpenAiService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = configuration["OpenAI:ApiKey"] ?? throw new Exception("API key not found");
		_organization = configuration["OpenAI:ApiOrganization"] ?? throw new Exception("API key not found");
		_projectId = configuration["OpenAI:ApiProjectId"] ?? throw new Exception("API key not found");
        _model = configuration["OpenAI:Model"] ?? "gpt-4";

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
		_httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", _organization);
		_httpClient.DefaultRequestHeaders.Add("OpenAI-Project", _projectId);
    }

    public async Task<RespostaGpt> GetCompletionAsync(PromptAcao prompt, List<(PromptAcao, RespostaGpt)> historico)
    {
        var messages = new List<GptMessage>
        {
            new GptMessage
            {
                Role = "system",
                Content = Prompt
            }
        };

        // Adiciona histórico
        if (historico != null && historico.Any())
        {
            foreach (var (userPrompt, assistantResponse) in historico)
            {
                messages.Add(new GptMessage
                {
                    Role = "user",
                    Content = JsonSerializer.Serialize(userPrompt)
                });
                messages.Add(new GptMessage
                {
                    Role = "assistant",
                    Content = JsonSerializer.Serialize(assistantResponse)
                });
            }
        }

        // Adiciona o prompt atual
        messages.Add(new GptMessage
        {
            Role = "user",
            Content = JsonSerializer.Serialize(prompt)
        });

        var requestBody = JsonSerializer.Serialize(new GptRequest
        {
            Model = _model,
            Messages = messages.ToArray(),
        });

        Console.WriteLine("-------------------------");
        Console.WriteLine(requestBody);

        using var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        using var responseMessage = await _httpClient.PostAsync(OpenAiApiUrl, content);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            throw new Exception($"Error calling OpenAI API: {responseMessage.ReasonPhrase}, Details: {errorContent}");
        }

        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        var root = JsonSerializer.Deserialize<JsonElement>(responseBody);

        var choices = root.GetProperty("choices");
        var message = choices.EnumerateArray().First()
            .GetProperty("message");
        var contentResponse = message.GetProperty("content")
            .GetString() ?? "";

        var respostaGpt = JsonSerializer.Deserialize<RespostaGpt>(contentResponse)
            ?? throw new Exception("Erro ao deserializar resposta do GPT");

        return respostaGpt;
    }
}
