namespace Radio.Core.Services
{
    public interface ISerializationService
    {
        string Serialize<T>(T value);

        T Deserialize<T>(string value);
    }
}
