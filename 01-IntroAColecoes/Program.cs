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

/*
 
    Vamos reforçar a importância do yield: 
    - quando usamos um IEnumerable, precisamos de um IEnumerator para percorrer a coleção.
    - quando implementamos IEnumerator (e seus membros MoveNext(), Current), acabamos criando uma coleção em memória, o que pode ser ineficiente.
    - o yield faz com que esse trabalho seja delegado para o runtime do .NET, em tempo de execução, somente quando necessário usar algum item da coleção.
    - por exemplo: quero usar a coleção DiasDaSemana para obter somente os três primeiros dias da semana.
    - sem o yield, precisaríamos criar uma coleção em memória, com todos os itens, tremendo desperdício!
    - com o yield, o enumerador só gera os dias quando necessário, economizando memória E processamento.
    - imagina uma coleção com milhares de itens, e você só precisa de alguns poucos?
    - outro exemplo: uma coleção que retorne números pares até um limite informado como parâmetro.
 
*/

static IEnumerable<int> NumerosPares(int limite)
{
    var resultado = new List<int>();
    for(int i=0; i <= limite; i++)
    {
        if (i %2 == 0) resultado.Add(i);
    }
    return resultado;
}

static IEnumerable<int> NumerosParesComYield(int limite)
{
    Console.WriteLine("Vai executar?");
    for (int i = 0; i <= limite; i++)
    {
        if (i % 2 == 0) yield return i;
    }
}

var pares = NumerosParesComYield(20000); // vai preparar os itens a serem enumerados... 

foreach (var par in pares) // ...só executa quando forem percorridos!
{
    Console.WriteLine(par);
    if (par >= 200) break; // ...e quando necessários (sob demanda; no caso, 200)
}

return;

/*
    Então o yield traz: legibilidade, simplicidade e eficiência (economia de memória e processamento).
    E quando podemos empregar o yield? Em métodos que retornam IEnumerable<T> ou IEnumerator<T>
*/


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