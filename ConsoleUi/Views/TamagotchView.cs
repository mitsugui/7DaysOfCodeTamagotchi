namespace ConsoleUi.Views;

internal class TamagotchiView
{
    public enum OpcoesMenuPrincipal
    {
        AdotarMascote,
        VerMascotes,
        Sair
    }

    public enum OpcoesMenuMascote : byte
    {
        SaberMais = 1,
        Adotar,
        Voltar
    }

    public IReadOnlyList<string>? Pokemons { get; set; }

    public string? NomeJogador { get; set; }

    public string? MascoteEscolhido { get; set; }

    public void MostrarBoasVindas()
    {
    }

    public void PedirNomeJogador()
    {
        Console.Clear();
        Console.WriteLine("Qual o seu nome?");

        NomeJogador = Console.ReadLine();
    }

    public OpcoesMenuPrincipal? MostrarMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine("---------------------- MENU ----------------------");
        Console.WriteLine($"{NomeJogador}. Você deseja:");
        Console.WriteLine("1. Adotar um mascote.");
        Console.WriteLine("2. Ver seus mascotes.");
        Console.WriteLine("3. Sair.");

        var opcao = Console.ReadLine();
        switch (opcao)
        {
            case "1":
                return OpcoesMenuPrincipal.AdotarMascote;
            case "2":
                return OpcoesMenuPrincipal.VerMascotes;
            case "3":
                return OpcoesMenuPrincipal.Sair;
            default:
                Console.WriteLine("Opção inválida.");
                return null;
        }
    }

    public void MostrarMenuEscolhaMascote()
    {
        if (Pokemons == null) return;

        Console.Clear();
        Console.WriteLine("---------------- ADOTAR UM MASCOTE ---------------");
        Console.WriteLine($"{NomeJogador}. Escolha uma espécie:");
        for (var i = 0; i < Pokemons.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {Pokemons[i]}");
        }
        
        var opcao = Console.ReadLine();
        if (int.TryParse(opcao, out var indiceEscolha)
            && indiceEscolha >= 1
            && indiceEscolha <= Pokemons.Count)
        {
            MascoteEscolhido = Pokemons[indiceEscolha - 1];
        }
        else
        {
            Console.WriteLine("Mascote inválido.");
        }
    }

    public OpcoesMenuMascote? MostrarMenuMascote()
    {
        Console.Clear();
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"{NomeJogador}. Você deseja:");
        Console.WriteLine($"1. Saber mais sobre: {MascoteEscolhido}.");
        Console.WriteLine($"2. Adotar: {MascoteEscolhido}.");
        Console.WriteLine("3. Voltar.");

        var opcao = Console.ReadLine();
        if (int.TryParse(opcao, out var indiceEscolha)
            && indiceEscolha > 0
            && indiceEscolha < 3)
        {
            return (OpcoesMenuMascote)indiceEscolha;
        }
        else
        {
            Console.WriteLine("Opção inválida.");
            return null;
        }
    }

    public void MostrarMascoteAdotado()
    {
        Console.WriteLine($"{NomeJogador}. Mascote adotado com sucesso, o ovo está chocando:");


        Console.WriteLine("");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    public void MostrarDetalhesDoMascote(PokemonResponse? mascote)
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

    public void MostrarMeusMascotes(IReadOnlyCollection<string> mascotesAdotados)
    {
        Console.Clear();
        if (mascotesAdotados.Count == 0)
        {
            Console.WriteLine($"{NomeJogador}. Você não adotou nenhum pokemon.");
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
}