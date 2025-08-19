/*
    Seja um arquivo com músicas em formato CSV (Comma Separated Values). 

    Implemente as funções abaixo:
    //     [x] Leia-o como uma coleção de músicas
    //     [x] Filtre a coleção por artista (por ex. Coldplay, Metallica, AC/DC)
    //     [x] Filtre a coleção por duração (por ex. maiores que 5 minutos)
    //     [x] Ordene a coleção por artista
    //     [x] Ordene a coleção por artista e em seguida por músicas com duração crescente
    //     [x] Recupere as 10 músicas mais longas
    //     [x] Crie uma coleção de artistas
    //     [x] Crie uma coleção de gêneros
    //     [ ] Crie uma coleção de artistas e suas músicas
    //     [ ] Informe a duração média das músicas da coleção
    //     [ ] Informe a duração total das músicas da coleção
    //     [ ] Informe qual artista tem mais músicas na coleção
    //     [ ] Filtre a coleção por gênero (por ex. rock)
 
*/

using var stream = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var leitor = new StreamReader(stream);

// a partir do yield, dos métodos de extensão, das expressões lambda
// é possível compor funções para obter novas coleções
// a partir de operações que manipulam coleções de entrada

// LINQ! Language Integrated Query
// permite separar a obtenção dos dados de origem
// das operações que os manipulam
//var musicas = MusicasDoCsv(leitor)          // coleção de origem
//    .Where(m => m.Titulo.StartsWith('T'))   // operação de filtro
//    .OrderBy(m => m.Artista)                // operação de ordenação  
//    .ThenBy(m => m.Duracao);                // outra operação de ordenação  

//var artistas = MusicasDoCsv(leitor)
//    .Select(m => m.Artista) // extrai os artistas de cada música (projeção)
//    .Distinct()
//    .OrderBy(a => a)
//    .SkipWhile(a => a.StartsWith('A') || a.StartsWith('B')); // traz os artistas que não começam com A e B
//    //.SkipWhile(m => m.Duracao < 400); // pega as primeiras músicas com duração menor que 400s

//foreach (var artista in artistas)
//{
//    Console.WriteLine(artista);
//}

//var generos = MusicasDoCsv(leitor)
//    .SelectMany(m => m.Generos)
//    .Distinct(); // extrai os gêneros de cada música (projeção)

//foreach (var genero in generos)
//{
//    Console.WriteLine(genero);
//}

//var musicas = MusicasDoCsv(leitor)
//    .DistinctBy(m => m.Artista); // extrai uma música por artista

//foreach (var musica in musicas)
//{
//    Console.WriteLine(musica);
//}

var duracaoMedia = MusicasDoCsv(leitor)
    .Average(m => m.Duracao); // média das durações das músicas

Console.WriteLine($"Duração média: {duracaoMedia} segundos");

var duracaoTotal = MusicasDoCsv(leitor)
    .Sum(m => m.Duracao); // soma das durações das músicas

Console.WriteLine($"Duração total: {duracaoTotal} segundos");

IEnumerable<Musica> MusicasDoCsv(StreamReader leitor)
{
    var linha = leitor.ReadLine();
    //var numLinha = 1;
    while (linha != null)
    {
        //Console.WriteLine($"\nProcessando linha {numLinha}...");
        var partes = linha.Split(';');
        if (partes.Length != 4) continue;
        var musica = new Musica
        {
            Titulo = partes[0],
            Artista = partes[1],
            Duracao = Convert.ToInt32(partes[2]),
            Generos = partes[3].Split(',').Select(g => g.Trim()).ToArray()
        };
        yield return musica;
        linha = leitor.ReadLine();
        //numLinha++;
    }
}

class Musica 
{
    public required string Titulo { get; set; }
    public required string Artista { get; set; }
    public required int Duracao { get; set; }
    public string[] Generos { get; set; } = [];

    public override string ToString()
    {
        return $"{Titulo} - {Artista} ({Duracao} seg)";
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is Musica outraMusica)
        {
            return this.Titulo.Equals(outraMusica.Titulo)
                && this.Artista.Equals(outraMusica.Artista);
        }
        return false; // se não for do tipo Musica, retorna false
    }

    public override int GetHashCode()
    {
        // comparação bit a bit dos dois números (gerados em Titulo e Artista), ajudando a misturá-los e garantir baixa probabilidade de colisão
        return this.Titulo.GetHashCode() ^ this.Artista.GetHashCode();
    }
}
