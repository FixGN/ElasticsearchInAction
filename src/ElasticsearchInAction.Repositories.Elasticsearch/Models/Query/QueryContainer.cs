namespace ElasticsearchInAction.Repositories.Elasticsearch.Models.Query;

public record QueryContainer
{
    public required IQueryType Query { get; init; }
}