using Newtonsoft.Json;

namespace todoCOM.Repository;

[Serializable]
public class RepositoryData
{
    [JsonProperty(nameof(totalTaskCount))] public int totalTaskCount { get; set; } = 0;
    [JsonProperty(nameof(categoryCount))] public int categoryCount { get; set; } = 0;
    [JsonProperty(nameof(selectedCategory))] public string selectedCategory { get; set; } = "_";
}