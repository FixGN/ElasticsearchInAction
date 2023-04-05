namespace ElasticsearchInAction.Repositories.Elasticsearch.Models;

public record Book
{
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required int Edition { get; init; }
    public required string Synopsys { get; init; }
    public required double AmazonRating { get; init; }
    public required DateOnly ReleaseDate { get; init; }
    public required string[] Tags { get; init; }
    public required bool BestSeller { get; init; }
    public required Dictionary<string, double> Prices { get; init; }
}