using System.Text.Json;
using ConsoleUi.Model;

namespace ConsoleUi.Data;

internal class RepositorioMascotes
{
    private const string _caminhoArquivo = "mascotes.json";
    private Dictionary<string, List<Tamagotchi>>? _mascotesAdotados = null;

    public async Task SalvarMascoteAdotadoAsync(string usuario, Tamagotchi mascote)
    {
        var mascotesAdotados = await CarregarMascotesAsync();
        if (mascotesAdotados.TryGetValue(usuario, out var mascotes))
        {
            mascotes.Add(mascote);
        }
        else
        {
            mascotesAdotados.Add(usuario, [mascote]);
        }
        await File.WriteAllTextAsync(_caminhoArquivo, JsonSerializer.Serialize(_mascotesAdotados));
    }

    public async Task<List<Tamagotchi>> ListarMascotesAdotadosAsync(string usuario)
    {
        var mascotesAdotados = await CarregarMascotesAsync();
        return mascotesAdotados.TryGetValue(usuario, out var mascotes)
            ? mascotes
            : [];
    }

    private async Task<Dictionary<string, List<Tamagotchi>>> CarregarMascotesAsync()
    {
        if (_mascotesAdotados != null)
        {
            return _mascotesAdotados;
        }

        if (!File.Exists(_caminhoArquivo))
        {   
            _mascotesAdotados = new();
            await File.WriteAllTextAsync(_caminhoArquivo, JsonSerializer.Serialize(_mascotesAdotados));
            return _mascotesAdotados;
        }

        var json = await File.ReadAllTextAsync(_caminhoArquivo);
        _mascotesAdotados = JsonSerializer
            .Deserialize<Dictionary<string, List<Tamagotchi>>>(json) ?? new();

        return _mascotesAdotados;
    }
}