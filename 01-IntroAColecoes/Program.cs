/*
    dias da semana, playlist de músicas, estante de livros, filmes em uma prateleira, carrinho de compras, turma em uma escola, funcionários em uma empresa

    como representar esses conceitos em C#?

    ou seja:
    var diasDaSemana = ?
    var playlist = ?
    var carrinhoDeCompras = ?
*/

using System.Collections;

string[] diasDaSemana = ["Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado"];

var musica1 = new Musica { Titulo = "Bohemian Rhapsody", Artista = "Queen" };
var musica2 = new Musica { Titulo = "Imagine", Artista = "John Lennon" };

List<Musica> playlist = [musica1, musica2];

var produto1 = new Produto { Id = "1", Nome = "Camiseta", Preco = 29.90m };
var produto2 = new Produto { Id = "2", Nome = "Calça Jeans", Preco = 89.90m };

// antigamente tínhamos que usar ArrayList..., 
ArrayList carrinho1 = [produto1, produto2, musica1, "Domingo"];
//...mas hoje em dia preferimos usar List<T> porque é mais seguro e tipado
List<Produto> carrinho2 = [produto1, produto2]; //List<T> vem de System.Collections.Generic

// existem outras maneiras de inicializar listas
var playlist2 = new List<Musica>
{
    new Musica { Titulo = "Hotel California", Artista = "Eagles" },
    new Musica { Titulo = "Stairway to Heaven", Artista = "Led Zeppelin" }
};

public class Produto
{
    public required string Id { get; set; }
    public required string Nome { get; set; }
    public decimal? Preco { get; set; }
}

public class Musica
{
    public required string Titulo { get; set; }
    public required string Artista { get; set; }
}
