namespace ElasticsearchInAction.Api.Models;

public record Book
{
    public required string Id { get; init; }
    public required double Score { get; init; }
    public required BookData Data { get; init; }
}