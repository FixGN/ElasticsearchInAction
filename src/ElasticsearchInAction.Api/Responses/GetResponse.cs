using System.Diagnostics.CodeAnalysis;
using ElasticsearchInAction.Api.Models;

namespace ElasticsearchInAction.Api.Responses;

public record GetResponse
{
    public Book? Book { get; init; }
}