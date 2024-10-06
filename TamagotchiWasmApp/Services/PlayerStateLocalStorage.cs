using Blazored.LocalStorage;

public class PlayerStateLocalStorage : IPlayerStateStorage
{
    private readonly ILocalStorageService _localStorage;

    public PlayerStateLocalStorage(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SetActivePlayerAsync(string nomeJogador)
    {
        await _localStorage.SetItemAsStringAsync("ActivePlayer", nomeJogador);
    }

    public async Task SavePlayerStateAsync(PlayerState playerState)
    {
        if (playerState == null) return;

        var players = await _localStorage.GetItemAsync<Dictionary<string, PlayerState>>("Players") ?? new();
        players[playerState.NomeJogador] = playerState;

        await _localStorage.SetItemAsync("Players", players);
    }

    public async Task<PlayerState?> LoadActivePlayerStateAsync()
    {
        var nomeJogador = await _localStorage.GetItemAsStringAsync("ActivePlayer");
        if (string.IsNullOrWhiteSpace(nomeJogador)) return null;

        var players = await _localStorage.GetItemAsync<Dictionary<string, PlayerState>>("Players");
        return players != null && players.TryGetValue(nomeJogador, out var playerState)
            ? playerState 
            : null;
    }
}
