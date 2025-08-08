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


// vamos imprimir a playlist no terminal
foreach (var musica in playlist)
{
    Console.WriteLine($"Título: {musica.Titulo}, Artista: {musica.Artista}");
}

/*
    como o foreach funciona por baixo dos panos? 
    a partir de um objeto responsável por "percorrer" a coleção: o enumerador.
    olha só:
*/

var enumerador = playlist.GetEnumerator();
while (enumerador.MoveNext())
{
    //a cada chamada de MoveNext(), o cursor avança para o próximo item. Current retorna o item atual.
    var musica = enumerador.Current;
    Console.WriteLine($"Título: {musica.Titulo}, Artista: {musica.Artista}");
}

/*
    esse objeto implementa a interface IEnumerator:
    https://learn.microsoft.com/pt-br/dotnet/api/system.collections.ienumerator
    Recapitulando: IEnumerator tem os métodos MoveNext() e Reset(), e a propriedade Current. 
*/

// esse código é muito verboso, então o C# nos permite usar o foreach para simplificar :-)

foreach (var musica in playlist) // pega o enumerador implicitamente, e usa MoveNext() e Current; LINDO!
{
    Console.WriteLine($"Título: {musica.Titulo}, Artista: {musica.Artista}");
}

/*
    Então, pra fica bem claro: foreach só pode ser usado em coleções que implementam IEnumerable:
    https://learn.microsoft.com/pt-br/dotnet/csharp/language-reference/statements/iteration-statements#the-foreach-statement

    IEnumerable é uma interface que representa uma coleção que pode ser enumerada ("percorrida")
    https://learn.microsoft.com/pt-br/dotnet/api/system.collections.generic.ienumerable-1

     se eu fizer:
     foreach (var item in musica1)
     {
         Console.WriteLine(item);
     }

    não vai funcionar, porque Musica não implementa IEnumerable.
    mas e se quisesse implementar a capacidade de enumeração em uma classe minha?
    por exemplo, uma classe DiasDaSemana que representa os dias da semana e eu quero poder usar foreach nela?
    aí eu teria que implementar IEnumerable<T> e criar um enumerador que implementa IEnumerator<T>.
    onde T é string, porque os dias da semana são strings.
*/

var semana = new DiasDaSemana();
foreach (var dia in semana)
{
    Console.WriteLine(dia);
}

public class DiasDaSemanaEnumerator : IEnumerator<string>
{
    private readonly string[] dias = ["Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado"];
    private int posicao = -1;

    public string Current
    {
        get
        {
            if (posicao <0 || posicao >= dias.Length)
            {
                throw new InvalidOperationException("Posição inválida.");
            }
            return dias[posicao];
        }
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        
    }

    public bool MoveNext()
    {
        posicao++;
        return posicao < dias.Length;
    }

    public void Reset()
    {
        posicao = -1;
    }
}

public class DiasDaSemana : IEnumerable<string>
{
    public IEnumerator<string> GetEnumerator()
    {
        // posicao = -1
        //MoveNext(): posicao = 0
        yield return "Domingo"; // Current: array[posicao]
        //MoveNext(): posicao = 1
        yield return "Segunda-feira"; // Current
        //MoveNext(): posicao = 2
        yield return "Terça-feira"; // Current
        yield return "Quarta-feira";
        yield return "Quinta-feira";
        yield return "Sexta-feira";
        yield return "Sábado";
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

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