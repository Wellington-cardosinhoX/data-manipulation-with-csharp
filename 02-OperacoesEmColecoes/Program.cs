/*
    Seja um aplicativo de gerenciamento de músicas onde os usuários
podem organizar suas faixas favoritas em playlists personalizadas. 
Para cada playlist, é essencial que o usuário tenha controle total sobre a 
sequência de reprodução das músicas, permitindo reordená-las livremente a qualquer momento.
Além disso, o aplicativo precisa oferecer a funcionalidade de reprodução aleatória para uma playlist específica,
proporcionando uma experiência de audição dinâmica e variada, sem, contudo, alterar a ordem original que o usuário definiu.
O desafio é criar uma estrutura robusta que suporte a adição e remoção eficiente de músicas,
a reordenação flexível dentro das playlists e a seleção de faixas tanto em modo sequencial quanto aleatório.

    Funções que vamos implementar:
    //     [x] Criar as classes para musicas e playlist
    //     [x] Listar músicas da playlist
    //     [x] Adicionar música à playlist
    //     [x] Obter uma música específica da playlist
    //     [x] Remover música da playlist
    //     [ ] Tocar músicas da playlist em modo aleatório 
    //     [ ] Reordenar músicas segundo alguma lógica específica (ex. duração)
    //     [ ] Uma playlist não pode ter músicas repetidas
    //     [ ] Exibir as 10 músicas mais tocadas em todas as playlists (ranking)
    //     [ ] Player de música com:
    //     [ ] - Fila de reprodução (para músicas avulsas e/ou playlists)
    //     [ ] - Histórico de reprodução
 
*/

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;


//Dictionary<string, int> idades = new Dictionary<string, int>();

//idades.Add("João", 20);

////idades["João"] = 30;

//foreach (var i in idades)
//{
//    Console.WriteLine($"{i.Key} = {i.Value}");
//}


var musica1 = new Musica { Titulo = "Que País é esse?", Artista = "Legião Urbana", Duracao = 350 };

var musica5 = new Musica { Titulo = "Zeca", Artista = "Zegião Urbana", Duracao = 100 };

var musica2 = new Musica { Titulo = "Faroeste Caboclo", Artista = "Legião Urbana", Duracao = 565 };

var musica3 = new Musica { Titulo = "Tempo Perdido", Artista = "Legião Urbana", Duracao = 300 };

var musica4 = new Musica { Titulo = "Amendas", Artista = "Aegião Urbana", Duracao = 323 };

var rockNacional = new Playlist { Nome = "Rock Nacional" };

var mario = new Playlist { Nome = "mario nacional" };

rockNacional.Add(musica1);

rockNacional.Add(musica2);

rockNacional.Add(musica3);

rockNacional.Add(musica4);

rockNacional.Add(musica5);

rockNacional.Add(new Musica { Titulo = "Amendas", Artista = "Aegião Urbana", Duracao = 323 });

//ExibirPlaylist(rockNacional);

rockNacional.OrdenarPorArtista();

//ExibirPlaylist(rockNacional);


ExibirMaisTocadas(rockNacional, mario);

void RemoverMusicaPeloTitulo(Playlist playlist, string titulo)
{
    var musicaEncontrada = playlist.ObterPeloTitulo(titulo);

    if (musicaEncontrada is not null)
    {
        Console.WriteLine("\nRemovendo música...");
        rockNacional.Remove(musicaEncontrada);
    }
    else
    {
        Console.WriteLine("\nMúsica não encontrada");
    }

}

void ExibirPlaylist(Playlist playlist)
{
    Console.WriteLine($"\n Tocando as músicas de {playlist.Nome}");

    foreach (var musica in playlist)
    {
        Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista} - {musica.Duracao} segundos)");
    }
}

void ExibirMusicaAleatoria(Playlist playlist)
{
    var musicaAleatoria = playlist.ObterAleatoria();

    if (musicaAleatoria is not null)
    {
        Console.WriteLine(musicaAleatoria.Titulo);
    }
    else
    {
        Console.WriteLine("Não encontrado");
    }   
}


void ExibirMaisTocadas(Playlist playlist1, Playlist playlist2)
{
    Dictionary<Musica, int> ranking = [];

    foreach (var musica in playlist1)
    {
        ranking.Add(musica, 1);
    }

    foreach (var musica in playlist2)
    {
        if (ranking.TryGetValue(musica, out int contagem))
        {
            contagem++;
            ranking[musica] = contagem;
        }
        else
        {
            ranking[musica] = 1;
        }
    }

    List<KeyValuePair<Musica, int>> top = new(ranking);

    top.Sort(new PorContagem());

    Console.WriteLine("\nTop 3 músicas mais incluidas nas playlists");

    int contador = 1;

    foreach (var par in top)
    {
        Console.WriteLine($"\t - {par.Key.Titulo}");
        contador++;

        if (contador > 3) break;
    }
    
}

var player = new PlayDeMusica();

player.AdicionarNaFila(rockNacional);

ExibirFila(player);

var proxima = player.ProximaMusicaDaFila();

if (proxima is not null)
{
    Console.WriteLine($"Tocando a musica: {proxima.Titulo}...");
}
else
{
    Console.WriteLine("Fila de reprodução vazia");
}

ExibirFila(player);

var ultimoElemento = player.PegarUltimoElemento();

if (ultimoElemento is not null)
{
    Console.WriteLine($"Último elemento dessa lista de música é: {ultimoElemento.Titulo}");
}

void ExibirFila(PlayDeMusica playDeMusica)
{
    Console.WriteLine("\nExibindo lista de produções: ");
    foreach (var musica in player.Fila())
    {
        Console.WriteLine($"\t - Tocando música agora: {musica.Titulo}");
    }
}

public class PorContagem : IComparer<KeyValuePair<Musica, int>>
{
    public int Compare(KeyValuePair<Musica, int> x, KeyValuePair<Musica, int> y)
    {
        return y.Value.CompareTo(x.Value);
    }
}

public class PorArtista : IComparer<Musica>
{
    public int Compare(Musica? x, Musica? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Artista.CompareTo(y.Artista);
    }
}

public class PorTitulo : IComparer<Musica>
{
    public int Compare(Musica? x, Musica? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Titulo.CompareTo(y.Titulo);
    }
}

public class Musica : IComparable
{
    public string Titulo { get; set; } = string.Empty;
    public string Artista { get; set; } = string.Empty;
    public int Duracao { get; set; }

    public int CompareTo(object? other) // iguais: 0; menor: -1; maior: 1
    {
        if (other is null) return -1;
        if (other is Musica outraMusica) return this.Duracao.CompareTo(outraMusica.Duracao);
        return -1;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj is Musica outraMusica) return this.Titulo.Equals(outraMusica.Titulo) && this.Artista.Equals(outraMusica.Artista);

        return false;
    }

    public override int GetHashCode()
    {
        return this.Titulo.GetHashCode() ^ this.Artista.GetHashCode();
    }
}

public class Playlist : ICollection<Musica>
{
    private HashSet<Musica> set = [];

    private List<Musica> lista = [];
    public string Nome { get; set; } = string.Empty;

    public int Count => lista.Count;

    public bool IsReadOnly => false;

    public void Add(Musica item)
    {
        if (set.Add(item))
        {
            lista.Add(item);
        }
    }

    public void Clear()
    {
        lista.Clear();
    }

    public bool Contains(Musica item)
    {
        return lista.Contains(item);
    }

    public void CopyTo(Musica[] array, int arrayIndex)
    {
        lista.CopyTo(array, arrayIndex);
    }

    public Musica? ObterAleatoria()
    {
        if (lista.Count == 0) return null;

        var random = new Random();
        var numeroAleatorio = random.Next(0, lista.Count - 1);
        return lista[numeroAleatorio];
    }

    public void OrdenarPorDuracao()
    {
        lista.Sort(); // duração ?
    }

    public void OrdenarPorArtista()
    {
        lista.Sort(new PorArtista());
    }

    public Musica? ObterPeloTitulo(string titulo)
    {
        foreach (var musica in lista)
        {
            if (musica.Titulo == titulo) return musica;
        }

        return null;
    }

    public IEnumerator<Musica> GetEnumerator()
    {
        return lista.GetEnumerator();
    }

    public bool Remove(Musica item)
    {
        return lista.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public class PlayDeMusica
{
    private Queue<Musica> fila = [];

    public void AdicionarNaFila(Musica musica)
    {
        fila.Enqueue(musica);
    }

    public void AdicionarNaFila(Playlist playlist)
    {
        foreach (var musica in playlist)
        {
            AdicionarNaFila(musica);
        }
    }

    public Musica? ProximaMusicaDaFila()
    {
        if (fila.Count == 0) return null;
        
        return fila.Dequeue();
    }

    public Musica? PegarUltimoElemento()
    {
        return fila.LastOrDefault();
    }

    public IEnumerable<Musica> Fila()
    {
        foreach (var musica in fila)
        {
            yield return musica; // yield retorna 1 musica por vez, visando performance e economia de memoria. Ele não retrnoatudo de uma vez
        } 
    }
}