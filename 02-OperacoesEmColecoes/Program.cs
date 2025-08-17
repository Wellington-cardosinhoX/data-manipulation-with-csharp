/*
    Seja um aplicativo de gerenciamento de músicas onde os usuários podem organizar suas faixas favoritas em playlists personalizadas. Para cada playlist, é essencial que o usuário tenha controle total sobre a sequência de reprodução das músicas, permitindo reordená-las livremente a qualquer momento. Além disso, o aplicativo precisa oferecer a funcionalidade de reprodução aleatória para uma playlist específica, proporcionando uma experiência de audição dinâmica e variada, sem, contudo, alterar a ordem original que o usuário definiu. O desafio é criar uma estrutura robusta que suporte a adição e remoção eficiente de músicas, a reordenação flexível dentro das playlists e a seleção de faixas tanto em modo sequencial quanto aleatório.

    Funções que vamos implementar:
    //     [x] Criar as classes para musicas e playlist
    //     [x] Listar músicas da playlist
    //     [x] Adicionar música à playlist
    //     [x] Obter uma música específica da playlist
    //     [x] Remover música da playlist
    //     [x] Tocar músicas da playlist em modo aleatório 
    //     [ ] Reordenar músicas segundo alguma lógica específica (ex. duração)
 
*/

using System.Collections;

var rockNacional = new Playlist { Nome = "Músicas de Rock nacionais" };

rockNacional.AdicionarMusica(new Musica { Titulo = "Tempo Perdido", Artista = "Legião Urbana", Duracao = 4.55 });
rockNacional.AdicionarMusica(new Musica { Titulo = "Pro Dia Nascer Feliz", Artista = "Barão Vermelho", Duracao = 3.45 });
rockNacional.AdicionarMusica(new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 5.30 });
rockNacional.AdicionarMusica(new Musica { Titulo = "Geração Coca-Cola", Artista = "Legião Urbana", Duracao = 3.50 });

TocarPlaylist(rockNacional);

//var tituloABuscar = "Pro Dia Nascer Feliz";
//var musicaEncontrada = rockNacional.ObterMusicaPorTitulo(tituloABuscar);
//if (musicaEncontrada != null)
//{
//    Console.WriteLine($"Música encontrada: {musicaEncontrada}");
//    rockNacional.RemoverMusicaPorTitulo(tituloABuscar);
//}
//else
//{
//    Console.WriteLine("Música não encontrada.");
//}

//TocarPlaylist(rockNacional);

var playlistAleatoria = rockNacional.ModoAleatorio();
TocarPlaylist(playlistAleatoria);

var playlistPorDuracao = rockNacional.OrdenadaPor(new PorDuracaoComparer());
TocarPlaylist(playlistPorDuracao);

var playlistPorTitulo = rockNacional.OrdenadaPor(new PorTituloComparer());
TocarPlaylist(playlistPorTitulo);


void TocarPlaylist(Playlist playlist)
{
    Console.WriteLine($"\nVocê está ouvindo a playlist '{playlist.Nome}' ({playlist.TotalDeMusicas} músicas)");
    foreach (var musica in playlist)
    {
        Console.WriteLine($"\t - {musica}");
    }
    Console.WriteLine("\nFim da playlist.\n");
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
}

class Playlist : IEnumerable<Musica>
{
    // Musica[] _musicas = new Musica[10]; poderíamos usar um array, mas precisaríamos gerenciar manualmente a expansão e compressão da coleção quando incluíssemos ou excluíssemos; existe uma estrutura mais flexível e eficiente (e Orientada a Objetos!) para isso: List<T>
    private List<Musica> _musicas = []; // ou new List<Musica>(); ou new();
    public required string Nome { get; set; }
    public void AdicionarMusica(Musica musica)
    {
        _musicas.Add(musica); // adiciona a música à lista
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
            _musicas.Remove(musicaEncontrada); // remove a música da lista
    }

    public int TotalDeMusicas => _musicas.Count; // propriedade de List<T> 

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

    public IEnumerator<Musica> GetEnumerator()
    {
        return _musicas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}