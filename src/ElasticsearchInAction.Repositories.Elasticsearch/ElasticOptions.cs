namespace ElasticsearchInAction.Repositories.Elasticsearch;

public class ElasticOptions
{
    public const string OptionsName = "Elasticsearch";
    public required Uri Host { get; init; }
    public required string User { get; init; }
    public required string Password { get; init; }
    public required string IndexName { get; init; }
}
