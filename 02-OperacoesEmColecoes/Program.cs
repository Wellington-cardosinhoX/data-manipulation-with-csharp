/*
    Seja um aplicativo de gerenciamento de músicas onde os usuários podem organizar suas faixas favoritas em playlists personalizadas. Para cada playlist, é essencial que o usuário tenha controle total sobre a sequência de reprodução das músicas, permitindo reordená-las livremente a qualquer momento. Além disso, o aplicativo precisa oferecer a funcionalidade de reprodução aleatória para uma playlist específica, proporcionando uma experiência de audição dinâmica e variada, sem, contudo, alterar a ordem original que o usuário definiu. O desafio é criar uma estrutura robusta que suporte a adição e remoção eficiente de músicas, a reordenação flexível dentro das playlists e a seleção de faixas tanto em modo sequencial quanto aleatório.

    Funções que vamos implementar:
    //     [x] Criar as classes para musicas e playlist
    //     [x] Listar músicas da playlist
    //     [x] Adicionar música à playlist
    //     [x] Obter uma música específica da playlist
    //     [x] Remover música da playlist
    //     [x] Tocar músicas da playlist em modo aleatório 
    //     [x] Reordenar músicas segundo alguma lógica específica (ex. duração)
    //     [x] Uma playlist não pode ter músicas repetidas
    //     [x] Exibir as 10 músicas mais tocadas em todas as playlists (ranking)
    //     [x] Player de música com:
    //     [x] - Fila de reprodução (para músicas avulsas e/ou playlists)
    //     [x] - Histórico de reprodução
 
*/

using System.Collections;

var rockNacional = new Playlist { Nome = "Músicas de Rock nacionais" };

rockNacional.AdicionarMusica(
    new Musica { Titulo = "Tempo Perdido", Artista = "Legião Urbana", Duracao = 4.55 },
    new Musica { Titulo = "Pro Dia Nascer Feliz", Artista = "Barão Vermelho", Duracao = 3.45 },
    new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 5.30 },
    new Musica { Titulo = "Geração Coca-Cola", Artista = "Legião Urbana", Duracao = 3.50 },
    new Musica { Titulo = "Geração Coca-Cola", Artista = "Legião Urbana", Duracao = 3.50 }
);

var playlistLegiaoUrbana = new Playlist { Nome = "Legião Urbana" };
playlistLegiaoUrbana.AdicionarMusica(
    new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 5.30 },
    new Musica { Titulo = "Faroeste Caboclo", Artista = "Legião Urbana", Duracao = 9.30 },
    new Musica { Titulo = "Que País É Este", Artista = "Legião Urbana", Duracao = 3.50 },
    new Musica { Titulo = "Há Tempos", Artista = "Legião Urbana", Duracao = 4.20 }
);


var player = new Player();
player.Adicionar(new Musica { Titulo = "Bohemian Rhapsody", Artista = "Queen", Duracao = 5.55 });
player.Adicionar(rockNacional);
player.ExibirFila();
player.ExibirHistorico();

player.Tocar(); 
player.ExibirFila();
player.ExibirHistorico();

player.Tocar();
player.ExibirFila();
player.ExibirHistorico();

player.Voltar();
player.ExibirFila();
player.ExibirHistorico();

player.Tocar();
player.ExibirFila();
player.ExibirHistorico();

void TocarPlaylist(Playlist playlist)
{
    Console.WriteLine($"\nVocê está ouvindo a playlist '{playlist.Nome}' ({playlist.Count} músicas)");
    foreach (var musica in playlist)
    {
        Console.WriteLine($"\t - {musica}");
    }
    Console.WriteLine("\nFim da playlist.\n");
}


void ExibirMusicasMaisTocadas(params Playlist[] playlists)
{
    // criar uma "planilha" contendo duas colunas:
    // - a música
    // - o total de vezes que ela foi incluída em playlists

    // a representação de uma planilha de duas colunas em C# é o Dictionary
    // para os valores que queremos => Dictionary<Musica, int>
    Dictionary<Musica, int> ranking = new();

    foreach (var playlist in playlists)
    {
        foreach (var musica in playlist)
        {
            if (ranking.TryGetValue(musica, out int value))
            {
                ranking[musica] = ++value;
            }
            else // música ainda não está no ranking
            {
                ranking[musica] = 1; // inclui a música com total 1
            }
        }
    }

    // ordenar o ranking por total de vezes que a música foi incluída sem usar LINQ

    Console.WriteLine("\n\nRanking das 10 músicas mais tocadas:");
    var rankingOrdenado = new List<KeyValuePair<Musica, int>>(ranking);
    rankingOrdenado.Sort(new PorTotalComparer()); 
    for (var i = 0; i < Math.Min(10, rankingOrdenado.Count); i++)
    {
        var parMusicaTotal = rankingOrdenado[i];
        Console.WriteLine($"{i + 1}º lugar - {parMusicaTotal.Key} ({parMusicaTotal.Value} vezes)");
    }

}

class PorDuracaoComparer : IComparer<Musica>
{
    public int Compare(Musica? x, Musica? y)
    {
        if (x is null && y is null) return 0; // ambos são nulos, considerados iguais
        if (x is null) return -1;
        if (y is null) return 1;
        return x.Duracao.CompareTo(y.Duracao); 
    }
}

class PorTituloComparer : IComparer<Musica>
{
       public int Compare(Musica? x, Musica? y)
    {
        if (x is null && y is null) return 0; // ambos são nulos, considerados iguais
        if (x is null) return -1;
        if (y is null) return 1;
        return x.Titulo.CompareTo(y.Titulo);
    }
}

class PorTotalComparer : IComparer<KeyValuePair<Musica, int>>
{
    public int Compare(KeyValuePair<Musica, int> menor, KeyValuePair<Musica, int> maior)
    {
        return maior.Value.CompareTo(menor.Value); // ordena do maior para o menor
    }
}

class Musica : IComparable<Musica>
{
    public required string Titulo { get; set; }
    public required string Artista { get; set; }
    public required double Duracao { get; set; }

    public int CompareTo(Musica? other)
    {
        if (other is null) return 1;
        //return Duracao.CompareTo(other.Duracao); // usa duração para comparar as músicas
        return Titulo.CompareTo(other.Titulo); // usa título para comparar as músicas
    }

    public override string ToString()
    {
        return $"{Titulo} - {Artista} ({Duracao} min)";
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

class Playlist : ICollection<Musica>
{
    // Musica[] _musicas = new Musica[10]; poderíamos usar um array, mas precisaríamos gerenciar manualmente a expansão e compressão da coleção quando incluíssemos ou excluíssemos; existe uma estrutura mais flexível e eficiente (e Orientada a Objetos!) para isso: List<T>
    private List<Musica> _musicas = []; // ou new List<Musica>(); ou new();
    private HashSet<Musica> _musicasSet = new(); // ou new HashSet<Musica>();

    public required string Nome { get; set; }
    public void AdicionarMusica(params Musica[] musicas)
    {
        foreach (var musica in musicas)
        {
            this.Add(musica); 
        }
    }
    public Musica? ObterMusicaPorTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo)) return null;

        foreach (var musica in _musicas)
        {
            if (musica.Titulo.Equals(titulo))
            {
                return musica; // retorna a música se o título for encontrado
            }
        }
        return null; // retorna null se a música não for encontrada
    }

    public void RemoverMusicaPorTitulo(string titulo)
    {
        var musicaEncontrada = ObterMusicaPorTitulo(titulo);
        if (musicaEncontrada is not null)
        {
            _musicasSet.Remove(musicaEncontrada); // remove a música do HashSet
            _musicas.Remove(musicaEncontrada); // remove a música da lista
        }
    }

    public int Count => _musicasSet.Count;

    public bool IsReadOnly => false;

    public Playlist OrdenadaPor(IComparer<Musica> comparador)
    {
        List<Musica> novaList = [.. _musicas];

        novaList.Sort(comparador);

        return new Playlist
        {
            Nome = $"{this.Nome} (Ordenada)",
            _musicas = novaList
        };
    }

    public Playlist ModoAleatorio()
    {
        // 1. Cria uma cópia da lista de músicas original.
        //    Isso é importante para não modificar a ordem da playlist original
        //    e para que a nova playlist aleatória contenha as mesmas músicas.
        List<Musica> shuffledList = [.. _musicas]; // ou new List<Musica>(_musicas); ou new(_musicas);
        Random random = new Random();

        // 2. Implementação do algoritmo Fisher-Yates para embaralhar a lista.
        //    Este algoritmo percorre a lista de trás para frente. Em cada passo,
        //    ele troca o elemento atual com um elemento selecionado aleatoriamente
        //    da parte "não embaralhada" da lista (do início até a posição atual).
        //    Isso garante que cada elemento é acessado e movido apenas uma vez,
        //    resultando em uma permutação aleatória de todos os elementos originais,
        //    sem repetições ou omissões.
        int n = shuffledList.Count;
        while (n > 1)
        {
            n--; // Decrementa n para que o índice aleatório seja gerado entre 0 e n (inclusive)

            // Gera um índice aleatório 'k' no intervalo [0, n].
            // random.Next(maxValue) gera um número inteiro não negativo menor que maxValue.
            // Então, random.Next(n + 1) gerará um índice de 0 a n.
            int k = random.Next(n + 1);

            // Realiza a troca:
            // Salva o elemento na posição aleatória 'k'.
            Musica value = shuffledList[k];
            // Move o elemento da posição 'n' (o último da parte não embaralhada) para a posição 'k'.
            shuffledList[k] = shuffledList[n];
            // Coloca o elemento que estava em 'k' na posição 'n'.
            shuffledList[n] = value;
        }

        // 3. Retorna uma nova instância de Playlist com a lista de músicas embaralhada.
        return new Playlist
        {
            Nome = $"{this.Nome} (Modo Aleatório)", 
            _musicas = shuffledList
        };
    }

    public void Add(Musica musica)
    {
        // adiciona a música ao HashSet, se ela não existir
        // se existir, retorna false e não adiciona
        if (_musicasSet.Add(musica))
        {
            _musicas.Add(musica); // adiciona a música à lista
        }
    }

    public void Clear()
    {
        _musicas.Clear();
        _musicasSet.Clear();
    }

    public bool Contains(Musica item)
    {
        return _musicasSet.Contains(item);
    }

    public void CopyTo(Musica[] array, int arrayIndex)
    {
        _musicas.CopyTo(array, arrayIndex);
    }

    public bool Remove(Musica musica)
    {
        if (_musicasSet.Remove(musica))
        {
            _musicas.Remove(musica); // remove a música da lista
            return true;
        }
        return false;
    }

    public IEnumerator<Musica> GetEnumerator()
    {
        return _musicas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class Player
{
    private Queue<Musica> _fila = [];
    private Stack<Musica> _historico = [];
    public void Adicionar(Musica musica)
    {
        _fila.Enqueue(musica);
    }

    public void Adicionar(Playlist playlist)
    {
        foreach (var musica in playlist)
        {
            _fila.Enqueue(musica);
        }
    }

    public void ExibirFila()
    {
        Console.WriteLine("\nFila de reprodução:");
        foreach (var musica in _fila)
        {
            Console.WriteLine($"\t-{musica}");
        }
    }

    public void ExibirHistorico()
    {
        Console.WriteLine("\nHistórico de reprodução:");
        foreach (var musica in _historico)
        {
            Console.WriteLine($"\t-{musica}");
        }
    }

    public void Tocar()
    {
        // toca a primeira música da fila
        var musica = _fila.Dequeue();
        Console.WriteLine($"\nTocando: {musica}");
        _historico.Push(musica); // adiciona a música ao histórico
    }

    public void Voltar()
    {
        // toca a primeira música do histórico
        var musica = _historico.Peek();
        Console.WriteLine($"\nVoltando e Tocando: {musica}");
    }

    public void Avancar()
    {
        _fila.Dequeue(); // remove a música da fila sem tocar
    }
}