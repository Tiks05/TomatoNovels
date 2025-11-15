using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.WriterInfo
{
    public class WriterWorksDataDto
    {
        [JsonPropertyName("works")]
        public List<WorkDto> Works { get; set; } = new();
    }
}
