using System.Text.Json;
using System.Text.Json.Serialization;
using ElasticsearchInAction.Repositories.Elasticsearch.Models.Query;

namespace ElasticsearchInAction.Repositories.Elasticsearch.Converters;

public class RangeQueryJsonConverter<T> : JsonConverter<RangeQuery<T>>
{
    public override RangeQuery<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, RangeQuery<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("range");
        writer.WriteStartObject();
        if (options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase)
        {
            writer.WritePropertyName(char.ToLower(value.FieldName[0]) + value.FieldName[1..]);
        }
        else
        {
            writer.WritePropertyName(value.FieldName);
        }
        JsonSerializer.Serialize(writer, value.RangeParams, options);
        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}