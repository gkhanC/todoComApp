using todoCOM.Entities;

namespace todoCOM.Utils;

public class CategorySeparator
{
    public string categoryName { get; set; }
    public List<TodoTask> taskListCompleted { get; set; } = new List<TodoTask>();
    public List<TodoTask> taskListUnCompleted { get; set; } = new List<TodoTask>();

    public int totalTaskCount => (taskListCompleted.Count + taskListUnCompleted.Count);

    public void AddTask(params TodoTask[] tasks)
    {
        var unComp = tasks.Where((x) => !x.isCompleted);
        var unCompOrdered = unComp.OrderBy((x) => x.Id).ToList();
        if (unCompOrdered.Count > 0)
            taskListUnCompleted.AddRange(unCompOrdered);

        var comp = tasks.Where((x) => x.isCompleted);
        var compOrdered = comp.OrderBy((x) => x.Id).ToList();
        if (compOrdered.Count > 0)
        {
            taskListCompleted.AddRange(compOrdered);
        }
    }

    public CategorySeparator(string categoryName = "Main")
    {
        this.categoryName = categoryName;
    }
}