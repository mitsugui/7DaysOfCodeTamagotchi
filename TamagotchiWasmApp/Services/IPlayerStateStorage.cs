public interface IPlayerStateStorage
{
    Task SetActivePlayerAsync(string nomeJogador);
    Task SavePlayerStateAsync(PlayerState playerState);
    Task<PlayerState?> LoadActivePlayerStateAsync();
}