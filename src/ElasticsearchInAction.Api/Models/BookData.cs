using Dto = ElasticsearchInAction.Repositories.Elasticsearch.Models;

namespace ElasticsearchInAction.Api.Models;

public record BookData
{
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required int Edition { get; init; }
    public required string Synopsys { get; init; }
    public required double AmazonRating { get; init; }
    public required DateTime ReleaseDate { get; init; }
    public required string[] Tags { get; init; }
    public required bool BestSeller { get; init; }
    public required Dictionary<BookCurrencies, double> Prices { get; init; }

    public static BookData FromDto(Dto.Book dto)
    {
        return new BookData
        {
            Title = dto.Title,
            Author = dto.Author,
            Edition = dto.Edition,
            Synopsys = dto.Synopsys,
            ReleaseDate = dto.ReleaseDate.ToDateTime(new TimeOnly()),
            AmazonRating = dto.AmazonRating,
            BestSeller = dto.BestSeller,
            Tags = dto.Tags,
            Prices = new Dictionary<BookCurrencies, double>(
                dto.Prices.Select(price =>
                    new KeyValuePair<BookCurrencies, double>(GetCurrencyFromString(price.Key), price.Value)))
        };
    }
    
    private static BookCurrencies GetCurrencyFromString(string currency) => currency switch
    {
        "usd" => BookCurrencies.usd,
        "eur" => BookCurrencies.eur,
        "gbp" => BookCurrencies.gbp,
        _ => throw new ArgumentOutOfRangeException(nameof(currency))
    };
}