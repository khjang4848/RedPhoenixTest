namespace RedPhoenix.Infrastructure.Serialization;

using Newtonsoft.Json;

public class JsonTextSerializer(JsonSerializer serializer) : ITextSerializer
{
    private readonly JsonSerializer _serializer = serializer ??
                                                  throw new ArgumentNullException(nameof(serializer));

    public JsonTextSerializer() : this(JsonSerializer.Create(
        new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        }))
    {
    }

    public string Serialize<T>(T data)
    {
        using var writer = new StringWriter();
        var jsonWriter = new JsonTextWriter(writer)
        {
            Formatting = Formatting.Indented
        };

        _serializer.Serialize(jsonWriter, data);

        return writer.ToString();
    }

    public object? Deserialize(string serialized)
    {
        if (serialized == null)
        {
            throw new ArgumentNullException(nameof(serialized));
        }

        using var reader = new StringReader(serialized);
        var jsonReader = new JsonTextReader(reader);

        return _serializer.Deserialize(jsonReader);
    }
}
