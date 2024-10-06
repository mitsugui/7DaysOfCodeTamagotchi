using TamagotchiApi.Service;
using TamagotchiApp.Shared.Model;

var builder = WebApplication.CreateBuilder(args);

// Configura o serviço OpenAiService
builder.Services.AddSingleton<OpenAiService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicione o CORS para permitir solicitações do cliente Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowBlazorClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/gpt/interact", async (PromptRequest request, OpenAiService openAiService) =>
{
    try
    {
        // Obtenha o histórico se necessário
        var historico = request.Historico?.Select(h => (h.Prompt, h.Resposta)).ToList() ?? new List<(PromptAcao, RespostaGpt)>();

        var respostaGpt = await openAiService.GetCompletionAsync(request.Prompt, historico);

        return Results.Ok(respostaGpt);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
});

app.Run();
