using Newtonsoft.Json;

namespace todoCOM.Entities;

[Serializable]
public class Categories
{
    [JsonProperty(nameof(categoryNames))] public List<string> categoryNames { get; set; } = new List<string>();
}