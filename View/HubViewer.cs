using System.Security.Cryptography.X509Certificates;
using todoCOM.Entities;
using todoCOM.Repository;
using todoCOM.Utils;

namespace todoCOM.View;

public class HubViewer : Viewer
{
    private TaskRepository _repository;
    private ConsoleColorSettings _color;
    private HubDirective _directive;

    public HubViewer(TaskRepository repo, ConsoleColorSettings colorSetting, HubDirective directive)
    {
        _repository = repo;
        _color = colorSetting;
        _directive = directive;
    }

    public override void Show()
    {
        var separator = new string('_', 113);
        var category = CreateCategorySeparators();


        Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
        Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
        Console.WriteLine(separator);

        if (category != null)
        {
            Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
            Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
            var headerBottom =
                $"UnCompleted Tasks: {category.taskListUnCompleted.Count} \t Completed Tasks: {category.taskListCompleted.Count}";
            Console.Write($"{headerBottom,-81}");

            Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
            Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
            var totalTask = "Total number of tasks:" + category.totalTaskCount.ToString();
            Console.Write($"{totalTask,-28}");
            Console.WriteLine("");
        }

        Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
        Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
        var header = $"TodoCOM Shows {_directive.ToString()} Tasks in \"Category: {category.categoryName}\"";
        Console.Write($"{header,-81}");

        if (category == null)
        {
            Console.ForegroundColor = _color.WarningNotifyColor.ForegroundColor;
            Console.BackgroundColor = _color.WarningNotifyColor.BackgroundColor;
            Console.WriteLine($"Category is empty.");
            return;
        }

        Console.WriteLine("");
        Console.WriteLine("");
        ShowHeader(_color);

        Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
        Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
        Console.WriteLine(separator);


        if (category != null)
        {

            if (_directive.Equals(HubDirective.All) || _directive.Equals(HubDirective.Completed))
            {
                if (category.taskListCompleted.Count > 0)
                {
                    if (_directive.Equals(HubDirective.All) || _directive.Equals(HubDirective.UnCompleted))
                        Console.WriteLine(separator);

                    foreach (var selected in category.taskListCompleted)
                    {
                        var taskViewer = new TaskViewer(selected, _color);
                        taskViewer.Show();
                    }
                }
            }

            if (_directive.Equals(HubDirective.All) || _directive.Equals(HubDirective.UnCompleted))
            {
                foreach (var selected in category.taskListUnCompleted)
                {
                    var taskViewer = new TaskViewer(selected, _color);
                    taskViewer.Show();
                }
            }

        }
    }

    public void ShowHeader(ConsoleColorSettings getColor)
    {
        var eTask = new TodoTask();

        Console.ForegroundColor = getColor.SeparatorColor.ForegroundColor;
        Console.BackgroundColor = getColor.SeparatorColor.BackgroundColor;

        Console.Write($"{nameof(eTask.Id),-3}" + "  ");

        Console.Write("Done" + " ");

        Console.Write($"<{nameof(eTask.Tag) + ">:",-6}" + "  ");

        Console.Write($"{nameof(eTask.Title),-60}" + "  ");

        Console.Write($"{nameof(eTask.Category),-10}" + "  ");

        Console.Write($"{nameof(eTask.CreateDate),8}" + "  ");

        Console.Write($"{nameof(eTask.DueDate),8}" + "\n");

        Console.ResetColor();
    }


    private CategorySeparator CreateCategorySeparators()
    {
        var selected = _repository.Tasks.Where((x) => x.Category == _repository.getSelectedCategory());
        var cat = new CategorySeparator();
        cat.categoryName = _repository.getSelectedCategory() ?? "_";

        foreach (var task in selected)
        {
            cat.AddTask(task);
        }

        return cat;
    }

    public enum HubDirective
    {
        All,
        UnCompleted,
        Completed
    }
}