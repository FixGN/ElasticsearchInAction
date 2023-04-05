namespace ElasticsearchInAction.Repositories.Elasticsearch.Models.Query;

public record RangeQuery<T> : IQueryType
{
    public required string FieldName { get; init; }
    public required RangeQueryParameter<T> RangeParams { get; init; }
}