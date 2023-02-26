using ElasticsearchInAction.Api.Models;

namespace ElasticsearchInAction.Api.Responses;

public record GetAllResponse
{
    public required bool IsSuccess { get; init; }
    public Book[]? Books { get; init; }
}