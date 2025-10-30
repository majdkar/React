
using SchoolV01.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace SchoolV01.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}