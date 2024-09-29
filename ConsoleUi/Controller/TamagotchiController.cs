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

    private string? _nomeJogador;
    private string? _nomeMascoteEscolhidoAdocao;
    private Tamagotchi? _mascoteEscolhidoInteragir;

    public TamagotchiController(TamagotchiView view, TamagotchiService service)
    {
        _view = view;
        _service = service;
    }

    public async Task JogarAsync()
    {
        _view.MostrarBoasVindas();

        _nomeJogador = _view.PedirNomeJogador();
        if (_nomeJogador == null) return;

        var mascotesAdotados = new List<Tamagotchi>();
        while (true)
        {
            var opcao = _view.MostrarMenuPrincipal(_nomeJogador);
            switch (opcao)
            {
                case TamagotchiView.OpcoesMenuPrincipal.AdotarMascote:
                    _nomeMascoteEscolhidoAdocao = _view.MostrarMenuEscolhaMascoteAdocao(_nomeJogador, Pokemons.Keys.ToList());
                    if (_nomeMascoteEscolhidoAdocao != null)
                    {
                        var url = Pokemons[_nomeMascoteEscolhidoAdocao];
                        var mascoteAdotado = await AdotarMascoteAsync(_nomeJogador, _nomeMascoteEscolhidoAdocao, url);
                        if (mascoteAdotado != null) mascotesAdotados.Add(mascoteAdotado);
                    }
                    break;
                case TamagotchiView.OpcoesMenuPrincipal.VerMascotes:
                    _mascoteEscolhidoInteragir = _view.MostrarMenuEscolhaMascoteInteragir(_nomeJogador, mascotesAdotados);
                    if (_mascoteEscolhidoInteragir != null)
                    {
                        InteragirComMascote(_nomeJogador, _mascoteEscolhidoInteragir);
                    }
                    break;
                case TamagotchiView.OpcoesMenuPrincipal.Sair:
                    return;
            }
        }
    }

    public async Task<Tamagotchi?> AdotarMascoteAsync(string nomeJogador, string nomeMascote, string url)
    {
        Tamagotchi? mascoteAdotado = null;
        while (mascoteAdotado == null)
        {
            var opcao = _view.MostrarMenuMascoteAdotar(nomeJogador, nomeMascote);
            switch (opcao)
            {
                case TamagotchiView.OpcoesMenuAdotarMascote.SaberMais:
                    var mascote = await _service.ObterMascoteAsync(url);
                    if (mascote != null) _view.MostrarDetalhesDoMascote(mascote);
                    break;
                case TamagotchiView.OpcoesMenuAdotarMascote.Adotar:
                    mascoteAdotado = await _service.ObterMascoteAsync(url);
                    _view.MostrarMensagemMascoteAdotado(nomeJogador);
                    break;
                case TamagotchiView.OpcoesMenuAdotarMascote.Voltar:
                    return null;
            }
        }

        return mascoteAdotado;
    }

    public void InteragirComMascote(string nomeJogador, Tamagotchi mascoteInteragir)
    {
        while (true)
        {
            var opcao = _view.MostrarMenuInteragirComMascote(nomeJogador, mascoteInteragir);
            switch (opcao)
            {
                case TamagotchiView.OpcoesMenuInteragirMascote.SaberMais:
                    _view.MostrarDetalhesDoMascote(mascoteInteragir);
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.Alimentar when mascoteInteragir.EstaDormindo:
                    _view.MostrarMensagemMascoteEstaDormindo(mascoteInteragir);
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.Alimentar:
                    mascoteInteragir.Alimentar();
                    _view.MostrarMensagemMascoteAlimentado(mascoteInteragir);
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.Brincar when mascoteInteragir.EstaDormindo:
                    _view.MostrarMensagemMascoteEstaDormindo(mascoteInteragir);
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.Brincar:
                    mascoteInteragir.Brincar();
                    _view.MostrarMensagemMascoteBrincou(mascoteInteragir);
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.DormirOuAcordar:
                    if (mascoteInteragir.EstaDormindo)
                    {
                        mascoteInteragir.Acordar();
                        _view.MostrarMensagemMascoteAcordou(mascoteInteragir);
                    }
                    else
                    {
                        mascoteInteragir.Dormir();
                        _view.MostrarMensagemMascoteDormiu(mascoteInteragir);
                    }
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.Voltar:
                    return;
                default:
                    break;
            }
        }
    }
}