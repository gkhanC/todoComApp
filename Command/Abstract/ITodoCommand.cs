namespace todoCOM.Command.Abstract;

public interface ITodoCommand
{
    /// <summary>
    /// Task's option string.
    /// </summary>
    public string optionString { get; }

    /// <summary>
    /// Task's description.
    /// </summary>
    public string descriptionString { get; }

    /// <summary>
    /// Task's alias
    /// </summary>
    public string aliasString { get; }
    public string result { get; }
}