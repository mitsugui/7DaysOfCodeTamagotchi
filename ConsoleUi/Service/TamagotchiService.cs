using RestSharp;
using ConsoleUi.Model;

namespace ConsoleUi.Service;

internal class TamagotchiService
{
    private const string BaseUrl = @"https://pokeapi.co/api/v2/";

    private readonly RestClient _client = new(BaseUrl);

    public async Task<Pokemon?> ObterMascoteAsync(string url)
    {
        return await _client.GetAsync<Pokemon>(new RestRequest(url), CancellationToken.None);
    }
}