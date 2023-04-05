using System.Text.Json.Serialization;

namespace ElasticsearchInAction.Repositories.Elasticsearch.Models.Query;

public record RangeQueryParameter<T>
{
    [JsonPropertyName("lt")] 
    public T? Lt { get; set; }

    [JsonPropertyName("gt")]
    public T? Gt { get; set; }
}