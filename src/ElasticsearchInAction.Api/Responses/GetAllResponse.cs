using System.Diagnostics.CodeAnalysis;
using ElasticsearchInAction.Api.Models;

namespace ElasticsearchInAction.Api.Responses;

public record GetAllResponse
{
    public required bool IsSuccess { get; init; }
    [MemberNotNullWhen(false, nameof(IsSuccess))]
    public string? ErrorMessage { get; init; }
    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public Book[]? Books { get; init; }

    public static GetAllResponse ErrorResponse(string errorMessage)
    {
        return new GetAllResponse
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
    
    public static GetAllResponse SuccessResponse(Book[] books)
    {
        return new GetAllResponse
        {
            IsSuccess = true,
            Books = books
        };
    }
}