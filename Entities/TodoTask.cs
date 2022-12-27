using System.Linq.Expressions;
using System.Reflection;
using todoCOM.Attributes;


namespace todoCOM.Entities;

public class TodoTask
{
    public TodoTask()
    {
    }

    public string Id { get; set; } = null;
    public bool isCompleted { get; set; } = false;
    public string Tag { get; set; } = null;
    public string Title { get; set; } = null;
    public string Category { get; set; } = null;
    public string CreateDate { get; set; } = null;
    public string DueDate { get; set; } = null;

    public string nullKey { get; private set; } = "_#null";

    public void SetNull()
    {
        Id = Tag = Title = Category = CreateDate = DueDate = nullKey;
        isCompleted = false;
    }
}