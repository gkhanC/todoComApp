using Newtonsoft.Json;

namespace todoCOM.Entities;

[Serializable]
public class Storage
{
    [JsonProperty(nameof(categoryName))]
    public string categoryName { get; set; } = "main";
    
    [JsonProperty(nameof(tasks))]
    public List<TodoTask> tasks { get; set; } = new List<TodoTask>();
}