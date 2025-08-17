/*
    Seja um aplicativo de gerenciamento de músicas onde os usuários podem organizar suas faixas favoritas em playlists personalizadas. Para cada playlist, é essencial que o usuário tenha controle total sobre a sequência de reprodução das músicas, permitindo reordená-las livremente a qualquer momento. Além disso, o aplicativo precisa oferecer a funcionalidade de reprodução aleatória para uma playlist específica, proporcionando uma experiência de audição dinâmica e variada, sem, contudo, alterar a ordem original que o usuário definiu. O desafio é criar uma estrutura robusta que suporte a adição e remoção eficiente de músicas, a reordenação flexível dentro das playlists e a seleção de faixas tanto em modo sequencial quanto aleatório.

    Funções que vamos implementar:
    //     [x] Criar as classes para musicas e playlist
    //     [x] Listar músicas da playlist
    //     [x] Adicionar música à playlist
    //     [ ] Obter uma música específica da playlist
    //     [ ] Remover música da playlist
    //     [ ] Reordenar músicas na playlist em modo aleatório 
    //     [ ] Reordenar músicas segundo alguma lógica específica (ex. duração)
 
*/

using System.Collections;

var rockNacional = new Playlist { Nome = "Músicas de Rock nacionais" };

rockNacional.AdicionarMusica(new Musica { Titulo = "Tempo Perdido", Artista = "Legião Urbana", Duracao = 4.55 });
rockNacional.AdicionarMusica(new Musica { Titulo = "Pro Dia Nascer Feliz", Artista = "Barão Vermelho", Duracao = 3.45 });
rockNacional.AdicionarMusica(new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 5.30 });
rockNacional.AdicionarMusica(new Musica { Titulo = "Geração Coca-Cola", Artista = "Legião Urbana", Duracao = 3.50 });

Console.WriteLine($"Você está ouvindo a playlist '{rockNacional.Nome}' ({rockNacional.TotalDeMusicas} músicas)");
foreach (var musica in rockNacional)
{
    Console.WriteLine($"\t - {musica}");
}

class Musica
{
    public required string Titulo { get; set; }
    public required string Artista { get; set; }
    public required double Duracao { get; set; }
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

    public int TotalDeMusicas => _musicas.Count; // propriedade de List<T> 

    public IEnumerator<Musica> GetEnumerator()
    {
        return _musicas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}