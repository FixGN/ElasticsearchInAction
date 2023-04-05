namespace ElasticsearchInAction.Api.Responses;

public record CreateResponse
{
    public required bool IsSuccess { get; init; }
    public required string DocumentId { get; init; }
}