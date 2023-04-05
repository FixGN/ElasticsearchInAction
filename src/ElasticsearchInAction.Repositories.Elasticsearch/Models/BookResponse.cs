namespace ElasticsearchInAction.Repositories.Elasticsearch.Models;

public record BookResponse
{
    public required string Id { get; init; }
    public required double Score { get; init; }
    public required Book Book { get; init; }
}