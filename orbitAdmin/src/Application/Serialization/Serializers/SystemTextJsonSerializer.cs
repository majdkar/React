using System.Text.Json;
using SchoolV01.Application.Interfaces.Serialization.Serializers;
using SchoolV01.Application.Serialization.Options;
using Microsoft.Extensions.Options;

namespace SchoolV01.Application.Serialization.Serializers
{
    public class SystemTextJsonSerializer(IOptions<SystemTextJsonOptions> options) : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options = options.Value.JsonSerializerOptions   ;

        public T Deserialize<T>(string data)
            => JsonSerializer.Deserialize<T>(data, _options);

        public string Serialize<T>(T data)
            => JsonSerializer.Serialize(data, _options);
    }
}