using RestSharp;

namespace ConsoleUi.Service;

internal class TamagotchiService
{
    private const string BaseUrl = @"https://pokeapi.co/api/v2/";

    private readonly RestClient _client = new(BaseUrl);

    public async Task<PokemonResponse?> ObterMascoteAsync(string url)
    {
        return await _client.GetAsync<PokemonResponse>(new RestRequest(url), CancellationToken.None);
    }


}