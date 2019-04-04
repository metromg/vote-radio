using System;
using Newtonsoft.Json;
using Radio.Core.Services;

namespace Radio.Infrastructure.Services
{
    public class JsonSerializationService : ISerializationService
    {
        private readonly JsonSerializerSettings _serializationSettings;

        public JsonSerializationService()
        {
            _serializationSettings = GetSettings();
        }

        public string Serialize<T>(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return JsonConvert.SerializeObject(value, _serializationSettings);
        }

        public T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value, _serializationSettings);
        }

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            return settings;
        }
    }
}
