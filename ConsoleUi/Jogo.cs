using RestSharp;

internal class Jogo
{
    private const string BaseUrl = @"https://pokeapi.co/api/v2/";

    private static readonly (string Nome, string Url)[] Pokemons =
    [
        (Nome: "Pikachu", Url: "pokemon/25/"),
        (Nome: "Bulbasaur", Url: "pokemon/1/"),
        (Nome: "Charmander", Url: "pokemon/4/"),
        (Nome: "Ivysaur", Url: "pokemon/2/"),
        (Nome: "Pidgeot", Url: "pokemon/18/"),
        (Nome: "Psyduck", Url: "pokemon/54/"),
        (Nome: "Squirtle", Url: "pokemon/7/"),
    ];

    public async Task IniciarAsync()
    {
        DarBoasVindas();

        var nomeJogador = PedirNomeJogador();
        if (nomeJogador == null)
        {
            return;
        }

        await MostrarMenuPrincipalAsync(nomeJogador);
    }

    public void DarBoasVindas()
    {

    }

    public static string? PedirNomeJogador()
    {
        Console.Clear();
        Console.WriteLine("Qual o seu nome?");
        var nomeJogador = string.Empty;
        while (string.IsNullOrWhiteSpace(nomeJogador))
        {
            nomeJogador = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nomeJogador))
            {
                Console.WriteLine("Digite um nome ou s para sair.");
            }
            else if (nomeJogador?.ToLower() == "s")
            {
                Console.WriteLine("Saindo...");
                return null;
            }
        }
        return nomeJogador;
    }

    public static async Task MostrarMenuPrincipalAsync(string nomeJogador)
    {
        var mascotesAdotados = new List<string>();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("---------------------- MENU ----------------------");
            Console.WriteLine($"{nomeJogador}. Você deseja:");
            Console.WriteLine("1. Adotar um mascote.");
            Console.WriteLine("2. Ver seus mascotes.");
            Console.WriteLine("3. Sair.");

            var opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    var mascoteAdotado = await MostrarMenuAdotarUmMascoteAsync(nomeJogador);
                    if (mascoteAdotado != null) mascotesAdotados.Add(mascoteAdotado);
                    break;
                case "2":
                    MostrarMeusMascotes(nomeJogador, mascotesAdotados);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    private static void MostrarMeusMascotes(string nomeJogador, IReadOnlyCollection<string> mascotesAdotados)
    {
        Console.Clear();
        if (mascotesAdotados.Count == 0)
        {
            Console.WriteLine($"{nomeJogador}. Você não adotou nenhum pokemon.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("---------------- MASCOTES ADOTADOS ---------------");
        foreach (var mascote in mascotesAdotados)
        {
            Console.WriteLine($"- {mascote}");
        }
        Console.ReadKey();
    }

    public static async Task<string?> MostrarMenuAdotarUmMascoteAsync(string nomeJogador)
    {
        Console.Clear();
        Console.WriteLine("---------------- ADOTAR UM MASCOTE ---------------");
        Console.WriteLine($"{nomeJogador}. Escolha uma espécie:");
        for (var i = 0; i < Pokemons.Length; i++)
        {
            Console.WriteLine($"{i + 1} - {Pokemons[i].Nome}");
        }

        var opcao = Console.ReadLine();
        if (int.TryParse(opcao, out var indiceEscolha)
            && indiceEscolha >= 1
            && indiceEscolha <= Pokemons.Length)
        {
            var (mascote, url) = Pokemons[indiceEscolha - 1];
            return await MostrarMenuMascoteAsync(nomeJogador, mascote, url);
        }
        else
        {
            Console.WriteLine("Mascote inválido.");
            return null;
        }
    }

    public static async Task<PokemonResponse?> ObterMascoteAsync(string url)
    {
        var client = new RestClient(BaseUrl);
        var request = new RestRequest(url);
        return await client.GetAsync<PokemonResponse>(request, CancellationToken.None);
    }

    public static async Task<string?> MostrarMenuMascoteAsync(string nomeJogador, string mascote, string url)
    {
        string? mascoteAdotado = null;
        while (mascoteAdotado == null)
        {
            Console.Clear();
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"{nomeJogador}. Você deseja:");
            Console.WriteLine($"1. Saber mais sobre: {mascote}.");
            Console.WriteLine($"2. Adotar: {mascote}.");
            Console.WriteLine("3. Voltar.");

            var opcao = Console.ReadLine();
            if (int.TryParse(opcao, out var indiceEscolha))
            {
                switch (indiceEscolha)
                {
                    case 1:
                        var mascoteResponse = await ObterMascoteAsync(url);
                        MostrarDetalhesDoMascote(mascoteResponse);
                        break;
                    case 2:
                        AdotarMascote(nomeJogador);
                        mascoteAdotado = mascote;
                        break;
                    case 3:
                        return null;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida.");
            }
        }

        return mascoteAdotado;
    }

    private static void AdotarMascote(string nomeJogador)
    {
        Console.WriteLine($"{nomeJogador}. Mascote adotado com sucesso, o ovo está chocando:");
        Console.ReadKey();
    }

    public static void MostrarDetalhesDoMascote(PokemonResponse? mascote)
    {
        Console.Clear();
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($@"Nome do Pokemon: {mascote?.Name}");
        Console.WriteLine($" Altura: {mascote?.Height}");
        Console.WriteLine($" Peso: {mascote?.Weight}");
        if (mascote?.Types != null)
        {
            Console.WriteLine($" Tipos:");
            foreach (var tipo in mascote.Types)
            {
                Console.WriteLine($" - {tipo.Type.Name}");
            }
        }
        if (mascote?.Abilities != null)
        {
            Console.WriteLine($" Habilidades:");
            foreach (var ability in mascote.Abilities)
            {
                Console.WriteLine($" - {ability.Ability.Name.ToUpper()}");
            }
        }

        Console.WriteLine("");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }
}