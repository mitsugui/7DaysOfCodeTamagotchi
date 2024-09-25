# Desafio Alura 7 Days Of Code - Tamagotchi

Desafio proposto no site https://7daysofcode.io/matricula/csharp

## Objetivo

Dia 1: Criar um projeto de console em C# e conectar á API https://pokeapi.co/ para listar opções de tamagotchis.

Para essa implementação, foram escolhidos os seguintes pokemons:

1. [Pikachu](https://pokeapi.co/api/v2/pokemon/25/)
2. [Bulbasaur](https://pokeapi.co/api/v2/pokemon/1/)
3. [Charmander](https://pokeapi.co/api/v2/pokemon/4/)
4. [Ivysaur](https://pokeapi.co/api/v2/pokemon/2/)
5. [Pidgeot](https://pokeapi.co/api/v2/pokemon/18/)
6. [Psyduck](https://pokeapi.co/api/v2/pokemon/54/)
7. [Squirtle](https://pokeapi.co/api/v2/pokemon/7/)

Será usada a biblioteca RestSharp para realizar as chamadas à API REST.

Dia 2: Desserializar resposta do serviço em classes C#.

Foram criadas DTOs dentro da pasta Responses para converter a resposta da API e imprimir o resultado de forma legível.


Dia 3: Organização dos menus da aplicação.

Foi criada uma classe para imprimir as opções do menu e responder às escolhas do usuário.

Dia 4: Organizar em Model, View e Controller.

Organização da lógica do jogo no Controller. Separação da impressão de mensagens e captura de opções para a view.