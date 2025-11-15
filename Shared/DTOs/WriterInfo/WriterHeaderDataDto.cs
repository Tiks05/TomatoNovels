using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.WriterInfo
{
    public class WriterHeaderDataDto
    {
        [JsonPropertyName("writer")]
        public WriterDto Writer { get; set; } = new();
    }
}
