namespace RedPhoenix.Infrastructure.Serialization;

public interface ITextSerializer
{
    string Serialize<T>(T data);
    object? Deserialize(string serialized);
}