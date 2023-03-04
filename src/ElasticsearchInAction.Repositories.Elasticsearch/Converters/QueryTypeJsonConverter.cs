using System.Text.Json;
using System.Text.Json.Serialization;
using ElasticsearchInAction.Repositories.Elasticsearch.Models.Query;

namespace ElasticsearchInAction.Repositories.Elasticsearch.Converters;

public class QueryTypeJsonConverter : JsonConverter<IQueryType>
{
    public override IQueryType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IQueryType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (IQueryType?)null, options);
                break;
            default:
                var type = value.GetType();
                JsonSerializer.Serialize(writer, value, type, options);
                break;
        }
    }
}