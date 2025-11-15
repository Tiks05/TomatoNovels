using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class BookRankingOut
    {
        [JsonPropertyName("plot_type")]
        public string PlotType { get; set; } = "";

        [JsonPropertyName("child")]
        public List<RankingBookOut> Child { get; set; } = new();

        [JsonPropertyName("new_child")]
        public List<RankingBookOut> NewChild { get; set; } = new();
    }
}
