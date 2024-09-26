using ConsoleUi.Service;
using ConsoleUi.Views;
using ConsoleUi.Model;

namespace ConsoleUi.Controller;

internal class TamagotchiController
{
    private static readonly Dictionary<string, string> Pokemons = new()
    {
        {"Pikachu", "pokemon/25/"},
        {"Bulbasaur", "pokemon/1/"},
        {"Charmander", "pokemon/4/"},
        {"Ivysaur", "pokemon/2/"},
        {"Pidgeot", "pokemon/18/"},
        {"Psyduck", "pokemon/54/"},
        {"Squirtle", "pokemon/7/"},
    };

    private readonly TamagotchiView _view;
    private readonly TamagotchiService _service;

    public TamagotchiController(TamagotchiView view, TamagotchiService service)
    {
        _view = view;
        _service = service;

        _view.Pokemons = Pokemons.Keys.ToList();
    }

    public async Task JogarAsync()
    {
        _view.MostrarBoasVindas();

        _view.PedirNomeJogador();
        if (_view.NomeJogador == null) return;

        var mascotesAdotados = new List<string>();
        while (true)
        {
            var opcao = _view.MostrarMenuPrincipal();
            switch (opcao)
            {
                case TamagotchiView.OpcoesMenuPrincipal.AdotarMascote:
                    _view.MostrarMenuEscolhaMascote();
                    if (_view.MascoteEscolhido != null)
                    {
                        var url = Pokemons[_view.MascoteEscolhido];
                        var mascoteAdotado = await MostrarMenuAdotarMascoteAsync(_view.MascoteEscolhido, url);
                        if (mascoteAdotado != null) mascotesAdotados.Add(mascoteAdotado);
                    }
                    break;
                case TamagotchiView.OpcoesMenuPrincipal.VerMascotes:
                    _view.MostrarMeusMascotes(mascotesAdotados);
                    break;
                case TamagotchiView.OpcoesMenuPrincipal.Sair:
                    return;
            }
        }
    }

    public async Task<string?> MostrarMenuAdotarMascoteAsync(string mascote, string url)
    {
        string? mascoteAdotado = null;
        while (mascoteAdotado == null)
        {
            var opcao = _view.MostrarMenuMascote();
            switch (opcao)
            {
                case TamagotchiView.OpcoesMenuMascote.SaberMais:
                    var mascoteResponse = await _service.ObterMascoteAsync(url);
                    _view.MostrarDetalhesDoMascote(mascoteResponse);
                    break;
                case TamagotchiView.OpcoesMenuMascote.Adotar:
                    _view.MostrarMascoteAdotado();
                    mascoteAdotado = mascote;
                    break;
                case TamagotchiView.OpcoesMenuMascote.Voltar:
                    return null;
            }
        }

        return mascoteAdotado;
    }
}