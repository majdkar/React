using System.Text.Json;
using SchoolV01.Application.Interfaces.Serialization.Options;

namespace SchoolV01.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}