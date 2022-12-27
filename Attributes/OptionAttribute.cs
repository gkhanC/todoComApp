using Newtonsoft.Json.Serialization;

namespace todoCOM.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class OptionAttribute : Attribute
{
    public string Option { get; set; } = "";
    public string Description { get; set; } = "";
    public string Alias { get; set; } = "";

    public OptionAttribute(string option, string description, string alias)
    {
    }
}