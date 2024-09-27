using ConsoleUi.Model;

namespace ConsoleUi.Views;

internal class TamagotchiView
{
    public enum OpcoesMenuPrincipal
    {
        AdotarMascote,
        VerMascotes,
        Sair
    }

    public enum OpcoesMenuAdotarMascote : byte
    {
        SaberMais = 1,
        Adotar = 2,
        Voltar = 3
    }

    public enum OpcoesMenuInteragirMascote : byte
    {
        SaberMais = 1,
        Alimentar,
        Brincar,
        Voltar
    }

    public void MostrarBoasVindas()
    {
    }

    public string? PedirNomeJogador()
    {
        Console.Clear();
        Console.WriteLine("Qual o seu nome?");

        return Console.ReadLine();
    }

    public OpcoesMenuPrincipal? MostrarMenuPrincipal(string nomeJogador)
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

    public string? MostrarMenuEscolhaMascoteAdocao(string nomeJogador, IReadOnlyList<string> pokemons)
    {
        Console.Clear();
        Console.WriteLine("---------------- ADOTAR UM MASCOTE ---------------");
        Console.WriteLine($"{nomeJogador}. Escolha uma espécie:");
        for (var i = 0; i < pokemons.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {pokemons[i]}");
        }

        var opcao = Console.ReadLine();
        if (int.TryParse(opcao, out var indiceEscolha)
            && indiceEscolha >= 1
            && indiceEscolha <= pokemons.Count)
        {
            return pokemons[indiceEscolha - 1];
        }
        else
        {
            Console.WriteLine("Mascote inválido.");
        }
        return null;
    }

    public OpcoesMenuAdotarMascote? MostrarMenuMascoteAdotar(string nomeJogador, string mascoteEscolhidoAdocao)
    {
        Console.Clear();
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"{nomeJogador}. Você deseja:");
        Console.WriteLine($"1. Saber mais sobre: {mascoteEscolhidoAdocao}.");
        Console.WriteLine($"2. Adotar: {mascoteEscolhidoAdocao}.");
        Console.WriteLine("3. Voltar.");

        var opcao = Console.ReadLine();
        if (int.TryParse(opcao, out var indiceEscolha)
            && indiceEscolha > 0
            && indiceEscolha <= 3)
        {
            return (OpcoesMenuAdotarMascote)indiceEscolha;
        }
        else
        {
            Console.WriteLine("Opção inválida.");
            return null;
        }
    }

    public void MostrarMensagemMascoteAdotado(string nomeJogador)
    {
        Console.WriteLine($"{nomeJogador}. Mascote adotado com sucesso, o ovo está chocando:");


        Console.WriteLine("");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    public Tamagotchi? MostrarMenuEscolhaMascoteInteragir(string nomeJogador, IReadOnlyList<Tamagotchi> mascotesAdotados)
    {
        if (mascotesAdotados.Count == 0)
        {
            Console.WriteLine($"{nomeJogador}. Você não adotou nenhum pokemon.");
            Console.ReadKey();
            return null;
        }

        Console.Clear();
        Console.WriteLine("------------------- SEUS MASCOTE -----------------");
        Console.WriteLine($"{nomeJogador}. Escolha um mascote para brincar:");
        for (var i = 0; i < mascotesAdotados.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {mascotesAdotados[i].Nome}");
        }

        var opcao = Console.ReadLine();
        if (int.TryParse(opcao, out var indiceEscolha)
            && indiceEscolha >= 1
            && indiceEscolha <= mascotesAdotados.Count)
        {
            return mascotesAdotados[indiceEscolha - 1];
        }
        else
        {
            Console.WriteLine("Mascote inválido.");
        }
        return null;
    }

    public OpcoesMenuInteragirMascote MostrarMenuInteragirComMascote(string nomeJogador, Tamagotchi mascoteEscolhido)
    {
        Console.Clear();
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"{nomeJogador}. Você deseja:");
        Console.WriteLine($"1. Saber mais sobre {mascoteEscolhido.Nome}.");
        Console.WriteLine($"2. Alimentar {mascoteEscolhido.Nome}.");
        Console.WriteLine($"3. Brincar com {mascoteEscolhido.Nome}.");
        Console.WriteLine("4. Voltar.");

        var opcao = Console.ReadLine();
        if (int.TryParse(opcao, out var indiceEscolha)
            && indiceEscolha > 0
            && indiceEscolha <= 4)
        {
            return (OpcoesMenuInteragirMascote)indiceEscolha;
        }
        else
        {
            Console.WriteLine("Opção inválida.");
            return OpcoesMenuInteragirMascote.Voltar;
        }
    }

    public void MostrarDetalhesDoMascote(Tamagotchi? mascote)
    {
        Console.Clear();
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($@"Nome do Pokemon: {mascote?.Nome}");
        Console.WriteLine($" Altura: {mascote?.Altura}");
        Console.WriteLine($" Peso: {mascote?.Peso}");

        Console.WriteLine($" Humor:{mascote?.StatusHumor}");
        Console.WriteLine($" Alimentação: {mascote?.StatusFome}");
        Console.WriteLine($" Sono: {mascote?.StatusSono}");

        if (mascote?.Tipos != null)
        {
            Console.WriteLine($" Tipos:");
            foreach (var tipo in mascote.Tipos)
            {
                Console.WriteLine($" - {tipo}");
            }
        }
        if (mascote?.Habilidades != null)
        {
            Console.WriteLine($" Habilidades:");
            foreach (var habilidade in mascote.Habilidades)
            {
                Console.WriteLine($" - {habilidade.ToUpper()}");
            }
        }

        Console.WriteLine("");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }
}