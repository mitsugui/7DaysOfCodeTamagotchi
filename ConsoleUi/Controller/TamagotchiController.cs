using ConsoleUi.Service;
using ConsoleUi.Views;
using ConsoleUi.Model;
using ConsoleUi.Data;

namespace ConsoleUi.Controller;

internal class TamagotchiController
{
    private readonly TamagotchiView _view;
    private readonly TamagotchiService _service;
    private readonly GptChatCompletionService _gptService;
    private readonly RepositorioMascotes _repositorio = new();

    private string? _nomeJogador;
    private string? _nomeMascoteEscolhidoAdocao;
    private Tamagotchi? _mascoteEscolhidoInteragir;

    public TamagotchiController(TamagotchiView view, TamagotchiService service)
    {
        _view = view;
        _service = service;
        _gptService = new GptChatCompletionService();
    }

    public async Task JogarAsync()
    {
        _view.MostrarBoasVindas();

        _nomeJogador = _view.PedirNomeJogador();
        if (_nomeJogador == null) return;

        while (true)
        {
            var opcao = _view.MostrarMenuPrincipal(_nomeJogador);
            switch (opcao)
            {
                case TamagotchiView.OpcoesMenuPrincipal.AdotarMascote:
                    var pokemons = _service.ListarPokemons();
                    _nomeMascoteEscolhidoAdocao = _view.MostrarMenuEscolhaMascoteAdocao(_nomeJogador, pokemons.ToList());
                    if (_nomeMascoteEscolhidoAdocao == null) break;

                    var url = _service.ObterUrlPokemon(_nomeMascoteEscolhidoAdocao);
                    if (url == null) break;

                    var mascoteAdotado = await AdotarMascoteAsync(_nomeJogador, _nomeMascoteEscolhidoAdocao, url);
                    if (mascoteAdotado != null) await _repositorio.SalvarMascoteAdotadoAsync(_nomeJogador, mascoteAdotado);
                    break;
                case TamagotchiView.OpcoesMenuPrincipal.VerMascotes:
                    var mascotesAdotados = await _repositorio.ListarMascotesAdotadosAsync(_nomeJogador);
                    _mascoteEscolhidoInteragir = _view.MostrarMenuEscolhaMascoteInteragir(_nomeJogador, mascotesAdotados);
                    if (_mascoteEscolhidoInteragir == null) break;

                    await InteragirComMascoteAsync(_nomeJogador, _mascoteEscolhidoInteragir);
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

    public async Task InteragirComMascoteAsync(string nomeJogador, Tamagotchi mascoteInteragir)
    {
        while (true)
        {
            var opcao = _view.MostrarMenuInteragirComMascote(nomeJogador, mascoteInteragir, out var outraAcao);

            var textoAcao = opcao == TamagotchiView.OpcoesMenuInteragirMascote.Outro
                ? outraAcao
                : opcao == TamagotchiView.OpcoesMenuInteragirMascote.DormirOuAcordar
                    ? mascoteInteragir.EstaDormindo
                        ? "Acordar"
                        : "Dormir"
                    : opcao.ToString();

            var prompt = new PromptAcao()
            {
                Acao = textoAcao,
                Humor = mascoteInteragir.Humor,
                Fome = mascoteInteragir.Fome,
                Sono = mascoteInteragir.Sono,
                EstaDormindo = mascoteInteragir.EstaDormindo
            };

            switch (opcao)
            {
                case TamagotchiView.OpcoesMenuInteragirMascote.SaberMais:
                    _view.MostrarDetalhesDoMascote(mascoteInteragir);
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.DormirOuAcordar:
                    _gptService.GetGpt4CompletionAsync(mascoteInteragir, prompt)
                        .GetAwaiter()
                        .GetResult();
                    mascoteInteragir.DormirOuAcordar();
                    await _repositorio.SalvarMascoteAdotadoAsync(nomeJogador, mascoteInteragir);
                    _view.MostrarMensagemInteracao(mascoteInteragir);
                    break;
                case TamagotchiView.OpcoesMenuInteragirMascote.Voltar:
                    return;
                default:
                    _gptService.GetGpt4CompletionAsync(mascoteInteragir, prompt)
                        .GetAwaiter()
                        .GetResult();
                    await _repositorio.SalvarMascoteAdotadoAsync(nomeJogador, mascoteInteragir);
                    _view.MostrarMensagemInteracao(mascoteInteragir);
                    break;
            }
        }
    }
}